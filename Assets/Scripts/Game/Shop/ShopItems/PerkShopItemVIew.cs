
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PerkShopItemView: MonoBehaviour, IShopItemView, IPointerClickHandler
{
    [SerializeField] private Image m_image;
    [SerializeField] private TMP_Text m_priceText;
    
    private PerkShopItem m_model;
    
    public event Action<IShopItemModel> OnItemClicked;
    
    public void Display(IShopItemModel model)
    {
        if (model == null)
        {
            Debug.Log("null model was pased");
            return;
        } 
        
        m_model = model as PerkShopItem;
        m_priceText.text = model.Price.GetPrice().ToString();
        m_image.sprite = m_model.PerkData.Icon;

        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_model != null)
        {
            OnItemClicked?.Invoke(m_model); 
            return;
        }
        
        Debug.Log("m_model was null when clicked on perK"); 
    }
}
