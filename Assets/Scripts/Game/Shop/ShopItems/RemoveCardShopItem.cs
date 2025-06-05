
using System;
using Unity.VisualScripting;
using UnityEngine;

public class RemoveCardShopItem : IShopItemModel
{
    public event Action OnDataChanged;
    public int NumOfPurchases { get; set; }
    public bool Sellable { get; set; } = true;
    public IPrice Price { get; set; }

    private const int m_priceIncrease = 10;
    ~RemoveCardShopItem()
    {
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;
    }
    
    public bool Purchase()
    {
        Debug.Log("one card removal purchased");
        
        bool success = false;
        if (!string.IsNullOrEmpty(GameSessionParams.deckTemplateClientId))
        {
            DeckTemplates.Deck template = DeckTemplates.FindById(GameSessionParams.deckTemplateClientId);
            GameplayEvents.SendShowCardsForSelecting(template.CardsInDeck);
            GameplayEvents.CardSelectedByPlayer += OnCardSelected;
            success = Price.ReduceCost();
            Price.SetPrice(Price.GetPrice() + m_priceIncrease);
            OnDataChanged?.Invoke();
        }
        else
        {
            Debug.Log("deck template id was null");
        }
        

        return success;
    }

   
    private void OnCardSelected(CardDisplay cardDisplay)
    {
        Debug.Log("selected this card: " + cardDisplay.CardInDeck.GetCardName());
        DeckTemplates.RemoveCardFromDeck(GameSessionParams.deckTemplateClientId, cardDisplay.CardInDeck.GetCardData());
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;
    }

    public void Configure(IPrice price)
    {
        Price = price;
    }
}
