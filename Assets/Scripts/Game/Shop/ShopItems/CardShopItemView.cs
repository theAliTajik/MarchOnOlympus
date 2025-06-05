
using System;
using TMPro;
using UnityEngine;

public class CardShopItemView : MonoBehaviour, IShopItemView
{
    [SerializeField] private TMP_Text m_priceText;
    private CardDisplay m_cardDisplay;
    private CardShopItem m_model;

    public event Action<IShopItemModel> OnItemClicked;

    public void Display(IShopItemModel model)
    {
        if (model == null)
        {
            Debug.Log("null model was passed");
            return;
        } 
        m_model = model as CardShopItem;
        
        m_cardDisplay = PoolCardDisplay.Instance.GetItem();
        
        m_cardDisplay.Configure(m_model.CardData);
        m_cardDisplay.transform.SetParent(transform, false);
        
        m_cardDisplay.transform.localScale = Vector3.one;
        m_cardDisplay.transform.localPosition = Vector3.zero;

        m_priceText.text = model.Price.GetPrice().ToString();

        m_cardDisplay.OnClick += OnClick;
    }

    private void OnClick(CardDisplay cardDisplay)
    {
        Debug.Log("clicked");
        OnItemClicked?.Invoke(m_model);
    }

}
