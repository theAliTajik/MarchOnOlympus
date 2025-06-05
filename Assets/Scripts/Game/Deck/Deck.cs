using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.ModifiableParam;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DrawPileType
{
    DRAW,
    HAND,
    DISCARD,
    PERISHED
}
public class Deck : MonoBehaviour
{
    public event Action<int> OnDrawPileChanged;
    public event Action<int> OnDiscardPileChanged;
    

    private CardPileFactory m_cardPileFactory;

    private Dictionary<CardStorage, CardPile> m_piles = new Dictionary<CardStorage, CardPile>();
    
    public Dictionary<CardStorage, CardPile> CardPiles => m_piles;

    
    
    [SerializeField] private ModifiableParam<int> m_drawAmount = new ModifiableParam<int>(5);



    private void Awake()
    {
        m_cardPileFactory = new CardPileFactory();
        m_piles = m_cardPileFactory.Piles;
    }

    
    public List<CardDisplay> GetAllCards()
    {
        List<CardDisplay> union = new List<CardDisplay>();
        for (int i = 0; i < (int)CardStorage.ALL; i++)
        {
            union.AddRange(m_piles[(CardStorage)i].Cards);
        }
        
        return union;
    }
    
    public List<CardDisplay> GetAllCardsIn(CardStorage cardStorage)
    {
        if (cardStorage == CardStorage.ALL)
        {
            return GetAllCards();
        }
        return m_piles[cardStorage].Cards;
    }

    public int CountAllCardsIn(CardStorage cardStorage)
    {
        return m_piles[cardStorage].Cards.Count;
    }

    public void DrawCards()
    {
        DrawCards(m_drawAmount);
    }
    public List<CardDisplay> DrawCards(int quantity)
    {
        // Debug.Log("card draw of this quanitity: " +quantity);
        if (quantity <= 0)
        {
            Debug.LogError("quantity less or equal to 0");
            return null;
        }

        int ingeniusPileCount = m_piles[CardStorage.INGENIUS].Cards.Count;
        if (ingeniusPileCount > 0)
        {
            List<CardDisplay> ingeniusCards = m_piles[CardStorage.INGENIUS].DrawCards(ingeniusPileCount);
            m_piles[CardStorage.DRAW_PILE].Cards.InsertRange(0, ingeniusCards);
        }

        if (quantity > m_piles[CardStorage.DRAW_PILE].Cards.Count)
        {
            MoveAllDiscardToDrawPile();
            m_piles[CardStorage.DRAW_PILE].Shuffle();
        }

        int drawPileCount = m_piles[CardStorage.DRAW_PILE].Cards.Count;
        if (quantity > drawPileCount)
        {
            Debug.Log("ERROR: number of cards in deck is too small");
        }
        int numOfcardsToDraw = Mathf.Min(quantity, drawPileCount);
        
        List<CardDisplay> drawnCards = new List<CardDisplay>();
        drawnCards.AddRange(m_piles[CardStorage.DRAW_PILE].DrawCards(numOfcardsToDraw));
        m_piles[CardStorage.HAND].Cards.InsertRange(0, drawnCards);
        
        SendDrawDiscardPileCountChange();
        return drawnCards;
    }
    public void MoveCardTo(CardDisplay card, CardStorage cardStorage)
    {
        for (int i = 0; i < (int)CardStorage.ALL; i++)
        {
            m_piles[(CardStorage)i].Cards.Remove(card);
        }
        
        m_piles[cardStorage].Cards.Add(card);

        SendDrawDiscardPileCountChange();
    }
    
    public void MoveAllDiscardToDrawPile()
    {
        if (m_piles[CardStorage.DISCARD_PILE].Cards.Count <= 0)
        {
            return;
        }
        
        m_piles[CardStorage.DRAW_PILE].Cards.AddRange(m_piles[CardStorage.DISCARD_PILE].DrawAllCards());

        
        SendDrawDiscardPileCountChange();
    }

    public void DiscardCurrentHand()
    {
        List<CardDisplay> m_currentHand = m_piles[CardStorage.HAND].Cards;
        for (int i = 0; i < m_currentHand.Count; i++)
        {
            if (m_currentHand[i].CardInDeck.CurrentState.DoesPerishIfNotUsed())
            {
                m_piles[CardStorage.PERISHED_PILE].Cards.Add(m_currentHand[i]);
            }
            else
            {
                m_piles[CardStorage.DISCARD_PILE].Cards.Add(m_currentHand[i]);
                //GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_DISCARDED);
            }
        }
        
        m_piles[CardStorage.HAND].Cards.Clear();
        SendDrawDiscardPileCountChange();
    }

    public void AddCard(CardDisplay card, CardStorage cardStorage)
    {
        if (cardStorage == CardStorage.ALL)
        {
            Debug.Log("Card added to Invalid storage Type");
            return;
        }
        m_piles[cardStorage].Cards.Add(card);
        SendDrawDiscardPileCountChange();
    }

    public void InsertCard(CardDisplay card, CardStorage cardStorage, int index)
    {
        m_piles[cardStorage].Cards.Insert(index, card);
    }
    
    public CardDisplay DicardCard()
    {
        if (m_piles[CardStorage.HAND].Cards.Count <= 0)
        {
            Debug.Log("no card available");
            return null;
        }
        CardDisplay card = m_piles[CardStorage.HAND].Cards[^1];
        MoveCardTo(card, CardStorage.DISCARD_PILE);
        return card;
    }

    public void Shuffle(CardStorage cardStorage)
    {
        m_piles[cardStorage].Shuffle();
    }


    
    public void SetDrawAmount(int amount)
    {
        if (amount < 0)
        {
            amount = 0;
        }
        m_drawAmount = amount;
    }

    
    public void SetDrawAmountOverride(IParamModifier<int> modifier)
    {
        // Debug.Log("draw amount modifier added");
        m_drawAmount.AddModifier(modifier);
    }

    public void RemoveDrawAmountOverride(IParamModifier<int> modifier)
    {
        // Debug.Log("modifier removed");
        m_drawAmount.RemoveModifier(modifier);
    }
    
    public void RemoveDrawAmountOverride()
    {
        m_drawAmount.RemoveAllModifiers();
    }

    private void SendDrawDiscardPileCountChange()
    {
        OnDrawPileChanged?.Invoke(m_piles[CardStorage.DRAW_PILE].Cards.Count);
        OnDiscardPileChanged?.Invoke(m_piles[CardStorage.DISCARD_PILE].Cards.Count);
    }

    public CardPile GetCardPile(CardStorage cardStorage)
    {
        if (cardStorage == CardStorage.ALL)
        {
            Debug.Log("Invalid card storage Type");
            return null;
        }
        return m_piles[cardStorage];
    }
    
}

public enum CardStorage
{
    INGENIUS,
    DRAW_PILE,
    HAND,
    PLAYING,
    DISCARD_PILE,
    PERISHED_PILE,
    ALL
}
