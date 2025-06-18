
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public Action<IShopItemModel> OnShopItemClicked;
    [SerializeField] private ShopItemViewFactory m_factory = new ShopItemViewFactory();
    [SerializeField] private ShopLayoutAssigner m_layoutAssigner = new ShopLayoutAssigner();

    [SerializeField] private CardDisplayList m_cardDisplayList;

    private void Start()
    {
        GameplayEvents.ShowCardsByData += m_cardDisplayList.ShowCards;
    }

    private void OnDestroy()
    {
        GameplayEvents.ShowCardsByData -= m_cardDisplayList.ShowCards;

    }

    public void DisplayShopItems(List<IShopItemModel> shopItems)
    {
        m_layoutAssigner.Config();
        m_factory.Config();
        for (var i = 0; i < shopItems.Count; i++)
        {
            DisplayShopItem(shopItems[i]);
        }
    }

    public void DisplayShopItem(IShopItemModel item)
    {
        //get correct view
        GameObject viewPrefab = m_factory.GetView(item);
        if (viewPrefab == null)
        {
            Debug.Log("view is null");
            return;
        }

        GameObject viewObj = Instantiate(viewPrefab, transform, false);
        IShopItemView view = viewObj.GetComponent<IShopItemView>();
        view.Display(item);

        Transform container = m_layoutAssigner.GetItemContainer(view);
        if (container == null)
        {
            Debug.Log("container null");
            return;
        }
        //Debug.Log("set object parent to be container: " + container.name);
        viewObj.transform.SetParent(container, false);
        
        view.OnItemClicked += OnItemClicked;
    }

    private void OnItemClicked(IShopItemModel item)
    {
        OnShopItemClicked?.Invoke(item);
    }
}
