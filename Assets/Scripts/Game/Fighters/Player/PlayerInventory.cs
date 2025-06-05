using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region Singleton

    private static PlayerInventory s_instance;
    public static PlayerInventory Instance => s_instance;

    void Awake()
    {
        s_instance = this;
    }

    void OnDestroy()
    {
        s_instance = null;
    }
    #endregion
    
    public List<RelicData> m_Relics = new List<RelicData>();
    public List<PotionsData> m_Potions = new List<PotionsData>();
    public int m_Coins;
    public int m_MaxEnergy;

    public bool CanAfford(int cost)
    {
        return false;
    }

    public bool Purchase(int cost)
    {
        return false;
    }

    public int GetCoins()
    {
        return m_Coins;
    }

    public void AddRelic(RelicData relic)
    {
        m_Relics.Add(relic);
    }

    public void LoseRelic(RelicData relic)
    {
        m_Relics.Remove(relic);
    }

    public void ClearRelics()
    {
        m_Relics.Clear();
    }

    public void AddPotion(PotionsData potion)
    {
        m_Potions.Add(potion);
    }

    public void UsePotion(PotionsData potion)
    {
        m_Potions.Remove(potion);
        //TODO:implement use of potion
    }

    public void LosePotion(PotionsData potion)
    {
        m_Potions.Remove(potion);
    }

    public void Clear()
    {
        m_Relics.Clear();
        m_Potions.Clear();
        //TODO: reset coin amount
    }
    
    
}
