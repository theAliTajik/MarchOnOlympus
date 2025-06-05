
using System;
using UnityEngine;

public class PerkShopItem : IShopItemModel
{
    private PerksDb.PerksInfo m_perkData;

    public event Action OnDataChanged;
    public int NumOfPurchases { get; set; }
    public bool Sellable { get; set; } = true;
    public IPrice Price { get; set; }

    public PerksDb.PerksInfo PerkData => m_perkData;

    public void Configure(PerksDb.PerksInfo perkData, IPrice price)
    {
        m_perkData = perkData;
        Price = price;
    }
    
    public bool Purchase()
    {
        //Debug.Log("perk purchased: " + m_perkData.clientID);

        if (GameProgress.Instance)
        {
            GameProgress.Instance.Data.PerkIds.Add(m_perkData.ClientID);
            GameProgress.Instance.Save();
        }
        return true;
    }
}
