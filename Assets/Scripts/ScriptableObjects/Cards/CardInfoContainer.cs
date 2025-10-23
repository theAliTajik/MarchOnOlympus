
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CardInfoContainer
{

    public List<CardsInfo> AllCards;
    
    [SerializeField] private string m_CharacterId;

    public CardsInfo FindById(string clientId)
    {
        clientId = clientId.ToLower();
        for(int i = 0; i < AllCards.Count; i++)
        {
            CardsInfo card = AllCards[i];
            if (card.clientID.ToLower() == clientId)
            {
                return card;
            }
        }

        return null;
    }

    public string GetID(BaseCardData cardData)
    {
        for(int i = 0; i < AllCards.Count; i++)
        {
            CardsInfo card = AllCards[i];
            if (card.CardData == cardData)
            {
                return card.clientID;
            }
        }
        
        return null;
    }

    public void CreateCard(BaseCardData cardData)
    {
        CardsInfo card = new CardsInfo();
        card.clientID = cardData.name;
        card.CardData = cardData;
        
        card.IsImplemented = true;
        
        AllCards.Add(card);
    }

    public BaseCardData GetRandom()
    {
        return AllCards[UnityEngine.Random.Range(0, AllCards.Count)].CardData;
    }

    public BaseCardData GetRandomLegen()
    {
        List<BaseCardData> legenCards = new List<BaseCardData>();
        for (var i = 0; i < AllCards.Count; i++)
        {
            if (AllCards[i].CardData.Rarity == CardRarity.LEGENDARY)
            {
                legenCards.Add(AllCards[i].CardData);
            }
        }
        
        return legenCards[UnityEngine.Random.Range(0, legenCards.Count)];
    }
    
    public BaseCardData GetRandomFromPacks(List<CardPacks> packs)
    {
        List<BaseCardData> cardsInPack = new List<BaseCardData>();
        for (int i = 0; i < AllCards.Count; i++)
        {
            foreach (CardPacks pack in packs)
            {
                if (pack == AllCards[i].CardData.CardPack)
                {
                    cardsInPack.Add(AllCards[i].CardData);
                    break;
                }
            }
        }

        return cardsInPack[UnityEngine.Random.Range(0, cardsInPack.Count)];
    }
    
    public List<BaseCardData> GetCardsWithName(string partialName, bool contains)
    {
        return AllCards
            .Where(c => c.clientID.IndexOf(partialName, StringComparison.OrdinalIgnoreCase) >= 0)
            .Select(c => c.CardData)
            .ToList();
    }

    public string GetCharId()
    {
        return m_CharacterId;
    }

    public List<BaseCardData> GetAllImplementedCards()
    {
        return AllCards.Where(card => card.IsImplemented)
            .Select(card => card.CardData).ToList();
    }
}
