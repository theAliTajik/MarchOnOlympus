
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopPrices", menuName = "Shop Prices")]
public class ShopPrices : ScriptableObject
{
    public ModifiableParam<int> CardCost = 5;
    public ModifiableParam<int> LegendaryCardCost = 15;

    public ModifiableParam<int> PerkCost = 15;
    public ModifiableParam<int> LegendaryPerkCost = 40;

    public ModifiableParam<int> RemoveCardCost = 10;
    public ModifiableParam<int> TransformCardCost = 5;

    public ModifiableParam<int> NumOfCards = 2;
    public ModifiableParam<int> NumOfLegenCards = 1;

    public ModifiableParam<int> NumOfPerks = 2;
    public ModifiableParam<int> NumOfLegenPerks = 1;

    private List<ModifiableParam<int>> m_allPrices = new List<ModifiableParam<int>>();

    private bool m_isInitializedList;
    public List<ModifiableParam<int>> GetAllPrices()
    {
        if (m_isInitializedList == true)
        {
            return m_allPrices;
        }

        m_allPrices.Add(CardCost);
        m_allPrices.Add(LegendaryCardCost);
        m_allPrices.Add(PerkCost);
        m_allPrices.Add(LegendaryPerkCost);
        m_allPrices.Add(RemoveCardCost);
        m_allPrices.Add(TransformCardCost);
        
        m_isInitializedList = true;
        
        return m_allPrices;
    }
}
