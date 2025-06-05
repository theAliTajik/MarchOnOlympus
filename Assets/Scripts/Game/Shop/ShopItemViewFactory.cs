using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ShopItemViewFactory
{
    private Dictionary<Type, GameObject> m_modelToViewMap = new();

    [SerializeField] private GameObject m_cardShopPrefab;
    [SerializeField] private GameObject m_perkShopPrefab;
    [SerializeField] private GameObject m_removeCardShopPrefab;
    [SerializeField] private GameObject m_transformRandCardShopPrefab;

    public void Config()
    {
        m_modelToViewMap.Add(typeof(CardShopItem), m_cardShopPrefab);
        m_modelToViewMap.Add(typeof(PerkShopItem), m_perkShopPrefab);
        m_modelToViewMap.Add(typeof(RemoveCardShopItem), m_removeCardShopPrefab);
        m_modelToViewMap.Add(typeof(TransformRandCardShopItem), m_transformRandCardShopPrefab);
    }
    
    public GameObject GetView(IShopItemModel model)
    {
        var modelType = model.GetType();
        //Debug.Log("model tupe: " + modelType.Name);
            
        return m_modelToViewMap.TryGetValue(modelType, out var viewType) ? viewType : null;
    }

}
