
using System;
using Unity.VisualScripting;
using UnityEngine;

public class CardShopItem : IShopItemModel
{
    private BaseCardData m_cardData;

    public event Action OnDataChanged;
    public int NumOfPurchases { get; set; }
    public bool Sellable { get; set; } = true;
    public IPrice Price { get; set; }
    public BaseCardData CardData => m_cardData;

    public void Configure(BaseCardData cardData, IPrice price)
    {
        m_cardData = cardData;
        Price = price;
    }
    
    public bool Purchase()
    {
        Debug.Log("Card purchased: " + m_cardData.Name);
        
        GameplayEvents.SendRewarderCardSelected(m_cardData);
        return Price.ReduceCost();
    }
}
