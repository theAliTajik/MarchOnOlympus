using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CardPile
{
    private List<CardDisplay> m_cards = new List<CardDisplay>();
    
    public List<CardDisplay> Cards => m_cards;

    public virtual List<CardDisplay> GetAllCards()
    {
        return m_cards;
    }

    public virtual int CardCount()
    {
        return m_cards.Count;
    }

    public virtual List<CardDisplay> DrawAllCards()
    {
        return DrawCards(m_cards.Count);
    }
    public virtual List<CardDisplay> DrawCards(int quantity)
    {
        if (quantity <= 0)
        {
            Debug.LogError("quantity less or equal to 0");
            return null;
        }

        if (quantity > m_cards.Count)
        {
            Debug.LogWarning("draw quantity is greater than card count in card pile: " + this.GetType().Name);
            quantity = m_cards.Count;
        }

        List<CardDisplay> cards = new List<CardDisplay>();
        for (int i = 0; i < quantity; i++)
        {
            cards.Insert(0, m_cards[0]);
            m_cards.RemoveAt(0);
        }
        return cards;
    }

    // public virtual void AddCard(CardDisplay card)
    // {
    //     m_cards.Add(card);
    // }
    //
    // public virtual void AddRange(List<CardDisplay> cards)
    // {
    //     m_cards.AddRange(cards);
    // }
    //
    // public virtual void RemoveCard(CardDisplay card)
    // {
    //     m_cards.Remove(card);
    // }
    //
    // public virtual void Clear()
    // {
    //     m_cards.Clear();
    // }


    public virtual void Shuffle()
    {
        ShuffleList(m_cards);
    }
    
        
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i
            int randomIndex = Random.Range(0, i + 1);

            // Swap elements
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}

public class IngeniusPile : CardPile {}
public class DrawPile : CardPile {}
public class CurrentHandPile : CardPile {}
public class PlayingQueuePile : CardPile {}
public class DiscardPile : CardPile {}
public class PerishedPile : CardPile {}


public class CardPileFactory
{
    private Dictionary<CardStorage, CardPile> m_piles = new Dictionary<CardStorage, CardPile>();

    public Dictionary<CardStorage, CardPile> Piles
    {
        get { return m_piles; }
    }

    public CardPileFactory()
    {
        m_piles.Add(CardStorage.INGENIUS, new IngeniusPile());
        m_piles.Add(CardStorage.DRAW_PILE, new DrawPile());
        m_piles.Add(CardStorage.HAND, new CurrentHandPile());
        m_piles.Add(CardStorage.PLAYING, new PlayingQueuePile());
        m_piles.Add(CardStorage.DISCARD_PILE, new DiscardPile());
        m_piles.Add(CardStorage.PERISHED_PILE, new PerishedPile());
    }
    
    
    public CardPile GetPile(CardStorage storage)
    {
        return m_piles[storage];
    }
}