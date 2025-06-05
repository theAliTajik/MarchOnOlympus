
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopLayoutAssigner
{
    private Dictionary<Type, Transform> m_tranforms = new Dictionary<Type, Transform>();
    
    [SerializeField] private Transform m_cardsContainer;
    [SerializeField] private Transform m_perksContainer;
    [SerializeField] private Transform m_removeCardContainer;
    [SerializeField] private Transform m_transformRandCardContainer;

    public void Config()
    {
        m_tranforms.Add(typeof(CardShopItemView), m_cardsContainer);
        m_tranforms.Add(typeof(PerkShopItemView), m_perksContainer);
        m_tranforms.Add(typeof(RemoveCardShopItemView), m_removeCardContainer);
        m_tranforms.Add(typeof(TransformRandCardShopItemView), m_transformRandCardContainer);
    }
    
    public Transform GetItemContainer<T>(T itemView)
    {
        //Debug.Log("this type wants a container: " + itemView.GetType());
        return m_tranforms[itemView.GetType()];
    }
    
}
