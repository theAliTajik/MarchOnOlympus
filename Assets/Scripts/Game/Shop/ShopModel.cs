
using System.Collections.Generic;
using Game.ModifiableParam;
using UnityEngine;

public class ShopModel : MonoBehaviour
{
    private const string m_modifiersPath = "/Progress/ShopModifiers.txt";
    
    [SerializeField] private ShopPrices m_shopPrices;

    private string m_path;
    private List<IShopItemModel> m_items = new List<IShopItemModel>();

    private List<IParamModifier<int>> m_modifiers = new List<IParamModifier<int>>();


    private readonly List<List<CardPacks>> m_cardPackPairings = new List<List<CardPacks>>
    {
        new List<CardPacks> { CardPacks.STR, CardPacks.BLOCK},
        new List<CardPacks> { CardPacks.BLEED, CardPacks.IMPALE},
        new List<CardPacks> { CardPacks.COMBO},
        new List<CardPacks> { CardPacks.STR, CardPacks.DEBUFF},
    };

    public void ApplyModifiers()
    {
        m_modifiers = GameProgress.Instance.Data.ShopModifiers;
        List<ModifiableParam<int>> allPrices = m_shopPrices.GetAllPrices();
        foreach (IParamModifier<int> modifier in m_modifiers)
        {
            for (int i = 0; i < allPrices.Count; i++)
            {
                allPrices[i].AddModifier(modifier);
            }
        }
    }
    
    public void GenerateShopItems()
    {
        for (int i = 0; i < m_shopPrices.NumOfCards; i++)
        {
            m_items.Add(GenerateRandCardShopItem());
        }

        for (int i = 0; i < m_shopPrices.NumOfLegenCards; i++)
        {
            m_items.Add(GenerateRandCardShopItem(true));
        }
        
        for (int i = 0; i < m_shopPrices.NumOfPerks; i++)
        {
            m_items.Add(GenerateRandPerkShopItem());
        }

        for (int i = 0; i < m_shopPrices.NumOfLegenPerks; i++)
        {
            m_items.Add(GenerateRandPerkShopItem(true));
        }
        
        
        m_items.Add(GenerateRemoveCardShopItem());
        m_items.Add(GenerateTransformCardShopItem());
    }
    
    public void GenerateAdvancedShopItems()
    {
        List<CardPacks> randCardPack = m_cardPackPairings[Random.Range(0, m_cardPackPairings.Count)];
        Debug.Log("rand card pack is: " + string.Join(", ", randCardPack));
        for (int i = 0; i < 3; i++)
        {
            m_items.Add(GenerateRandCardShopItemFromPack(randCardPack));
        }
        m_items.Add(GenerateRemoveCardShopItem());       
        m_items.Add(GenerateTransformCardShopItem());
    }


    private IShopItemModel GenerateTransformCardShopItem()
    {
        TransformRandCardShopItem item = new TransformRandCardShopItem();
        IPrice price = GenerateHonorPrice(m_shopPrices.TransformCardCost);
        item.Configure(price);
        return item;
    }

    
    public List<IShopItemModel> GetItems()
    {
        return m_items;
    }

    
    private HashSet<string> m_usedCards = new HashSet<string>(); 
    public CardShopItem GenerateRandCardShopItem(bool legendary = false)
    {
        CardShopItem item = new CardShopItem();
        BaseCardData data;
        IPrice price;
        int attempts = 10;
        do
        {
            if (legendary)
            {
                data = CardsDb.Instance.GetRandomLegen();
                price = GenerateHonorPrice(m_shopPrices.LegendaryCardCost);
            }
            else
            {
                data = CardsDb.Instance.GetRandom();
                price = GenerateHonorPrice(m_shopPrices.CardCost);
            }
            attempts--;
        } while (m_usedCards.Contains(data.Name) && attempts > 0);
        m_usedCards.Add(data.Name);
        
        item.Configure(data, price);
        return item;
    }
    
    public CardShopItem GenerateRandCardShopItemFromPack(List<CardPacks> packs)
    {
        CardShopItem item = new CardShopItem();
        BaseCardData data;
        int attempts = 10;
        do
        {
            data = CardsDb.Instance.GetRandomFromPacks(packs);
            attempts--;
        } while (m_usedCards.Contains(data.Name) && attempts > 0);
        m_usedCards.Add(data.Name);

        IPrice price;
        if (data.Rarity == CardRarity.LEGENDARY)
        {
            price = GenerateHonorPrice(m_shopPrices.LegendaryCardCost);
        }
        else
        {
            price = GenerateHonorPrice(m_shopPrices.CardCost);
        }
         
        item.Configure(data, price);
        return item;
    }
   
    private HashSet<string> m_usedPerks = new HashSet<string>();

    public PerkShopItem GenerateRandPerkShopItem(bool legendary = false)
    {
        PerkShopItem item = new PerkShopItem();
        PerksDb.PerksInfo randPerkData;
        IPrice price;
        int attempts = 10;
        do
        {
            if (legendary)
            {
                randPerkData = PerksDb.Instance.GetRandLegenPerk();
                price = GenerateHonorPrice(m_shopPrices.LegendaryPerkCost);
            }
            else
            {
                randPerkData = PerksDb.Instance.GetRandPerk();
                price = GenerateHonorPrice(m_shopPrices.CardCost);
            }
            attempts--;
        } while (m_usedPerks.Contains(randPerkData.ClientID) && attempts > 0);
        m_usedPerks.Add(randPerkData.ClientID);
        
        item.Configure(randPerkData, price);
        return item;
    }

    private RemoveCardShopItem GenerateRemoveCardShopItem()
    {
        RemoveCardShopItem item = new RemoveCardShopItem();
        item.Configure(GenerateHonorPrice(m_shopPrices.RemoveCardCost));
        return item;
    }
    private IPrice GenerateHonorPrice(int price)
    {
        IPrice itemPrice = new HonorPrice();
        itemPrice.SetPrice(price);
        return itemPrice;
    }
    
    
}       