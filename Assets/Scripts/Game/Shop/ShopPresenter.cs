
using System;
using UnityEngine;

public class ShopPresenter : MonoBehaviour
{
    [SerializeField] private ShopModel m_shopModel;
    [SerializeField] private ShopView m_shopView;

    [SerializeField] private bool m_isAdvancedShop;

    private void Start()
    {
        m_shopModel.ApplyModifiers();
        if (m_isAdvancedShop)
        {
            m_shopModel.GenerateAdvancedShopItems();
        }
        else
        {
            m_shopModel.GenerateShopItems();
        }
        m_shopView.DisplayShopItems(m_shopModel.GetItems()); 
        m_shopView.OnShopItemClicked += OnShopItemClicked;
        GameProgress.Instance.Data.ShopModifiers.Clear();
    }

    private void OnDestroy()
    {
        if (m_shopView != null)
        {
            m_shopView.OnShopItemClicked -= OnShopItemClicked;
        }
    }

    private void OnShopItemClicked(IShopItemModel item)
    {
        if (item == null)
        {
            Debug.Log("item was null");
            return;
        }

        if (item.Sellable && item.Price.HasEnoughMoney())
        {
            Debug.Log("item purchased succesfully");
            item.Purchase();
        }
        else
        {
            Debug.Log("item was not purchased succesfully");
        }
    }
}