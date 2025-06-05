
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransformRandCardShopItemView : MonoBehaviour, IShopItemView, IPointerClickHandler
{
    [SerializeField] private TMP_Text m_priceText;
    [SerializeField] private TMP_Text m_randCardText;
    [SerializeField] private TMP_Text m_newCardText;

    private TransformRandCardShopItem m_model;
    public event Action<IShopItemModel> OnItemClicked;
    
    public void Display(IShopItemModel model)
    {
        if (model == null)
        {
            Debug.Log("null model was passed");
            return;
        }    
        
        m_model = model as TransformRandCardShopItem;
        m_model.OnDataChanged += OnDataChanged;
        
        m_priceText.text = model.Price.GetPrice().ToString();
        
        m_randCardText.gameObject.SetActive(false);
        m_newCardText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemClicked?.Invoke(m_model);
    }

    private void OnDataChanged()
    {
        Refresh();
    }

    private void Refresh()
    {
        m_priceText.text = m_model.Price.GetPrice().ToString();

        if (m_model.RandCardName != null)
        {
            m_randCardText.text = m_model.RandCardName;
            m_newCardText.text = m_model.NewCardName;
            
            m_randCardText.gameObject.SetActive(true);
            m_newCardText.gameObject.SetActive(true);
        }
    }
}
