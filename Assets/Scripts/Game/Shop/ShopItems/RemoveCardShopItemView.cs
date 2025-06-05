
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveCardShopItemView : MonoBehaviour, IShopItemView , IPointerClickHandler
{
    private RemoveCardShopItem m_model;
    [SerializeField] private TMP_Text m_priceText;
    
    public event Action<IShopItemModel> OnItemClicked;
    public void Display(IShopItemModel model)
    {
        m_model = model as RemoveCardShopItem;
        m_priceText.text = model.Price.GetPrice().ToString();
        m_model.OnDataChanged += OnDataChanged;
    }

    private void OnDataChanged()
    {
        Refresh();
    }

    private void Refresh()
    {
        m_priceText.text = m_model.Price.GetPrice().ToString();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("registerd click");
        if (m_model != null)
        {
            OnItemClicked?.Invoke(m_model);
        }
    }
}
