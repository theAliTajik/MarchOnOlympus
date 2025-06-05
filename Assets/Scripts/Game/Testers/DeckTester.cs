using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckTester : MonoBehaviour
{
    public Deck deck;
    public List<BaseCardData> cards;

    private void Start()
    {
        logAllCardsInDeck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("draw");
            deck.DrawCards(5);
            logAllCardsInDeck();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("discard");
            deck.DiscardCurrentHand();
            logAllCardsInDeck();
        }
    }

    void logAllCardsInDeck()
    {
        // Debug.Log("Draw pile: " + string.Join(", ", deck.DrawPile));
        // Debug.Log("Current hand: " + string.Join(", ", deck.CurrentHand));
        // Debug.Log("Discard pile: " + string.Join(", ", deck.DiscardPile));
    }
}
