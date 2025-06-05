

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransformRandCardShopItem : IShopItemModel
{
    public event Action OnDataChanged;
    public int NumOfPurchases { get; set; }
    public bool Sellable { get; set; } = true;
    
    public IPrice Price { get; set; }

    private string m_randCardName;
    private string m_newCardName;
    
    public string RandCardName => m_randCardName;
    public string NewCardName => m_newCardName;
    
    private BaseCardData m_cardData;
    public void Configure(IPrice price)
    {
        Price = price;
    }
    
    public bool Purchase()
    {
        string deckId = GameSessionParams.deckTemplateClientId;
        if (!string.IsNullOrEmpty(deckId))
        {
            DeckTemplates.Deck template = DeckTemplates.FindById(deckId);
            
            int randCardIndex = Random.Range(0, template.CardsInDeck.Count);
            CardInDeckStateMachine randCard = template.CardsInDeck[randCardIndex];
            
            BaseCardData newCard;
            int attempts = 10;
            do
            {
                newCard = CardsDb.Instance.GetRandom();
                attempts--;
            } while (newCard.Name == randCard.GetCardName() && attempts > 0);
            
            DeckTemplates.RemoveCardFromDeck(deckId, randCard);
            DeckTemplates.AddCardToDeck(deckId, newCard);
            
            Debug.Log("transform purchased: turned: " + randCard.GetCardName() + " into: " + newCard.Name);
            m_randCardName = randCard.GetCardName();
            m_newCardName = newCard.Name;
            Sellable = false;
            OnDataChanged?.Invoke();
        }
        else
        {
            Debug.Log("deck template id was null");
        }
        
        return Price.ReduceCost();
    }
}

