using System.Collections;
using System.Collections.Generic;
using Game.ModifiableParam;
using UnityEngine;

public class DeckTemplateTester : MonoBehaviour
{
    private string m_testDeckId = "Test Deck";
    [SerializeField] private BaseCardData m_testCard;
    private DeckTemplates.Deck m_deck;
    
    private void Start()
    {
        DeckTemplates.LoadAllDecks();

        RemoveDeck();
        MakeDeck();
        DeckTemplates.LoadAllDecks();
        Debug.Log("reload decks");
        m_deck = DeckTemplates.FindById(m_testDeckId);

        CardInDeckStateMachine card = MakeCardInDeck();
        GiveCardModifiers(card);
        DeckTemplates.Save();
        LogDeck();
        
        DeckTemplates.LoadAllDecks();
        Debug.Log("reload decks");
        m_deck = DeckTemplates.FindById(m_testDeckId);
        LogDeck();

        card = MakeCardInDeck();
        GiveCardSecondModifiers(card);
        DeckTemplates.Save();
        
        Debug.Log("reload decks");
        m_deck = DeckTemplates.FindById(m_testDeckId);
        LogDeck();
    }


    private CardInDeckStateMachine MakeCardInDeck()
    {
        CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
        cardInDeck.Configure(m_testCard);
        DeckTemplates.AddCardToDeck(m_testDeckId, cardInDeck, -1);
        return cardInDeck;
    }

    private void RemoveDeck()
    {
        DeckTemplates.RemoveDeck(m_testDeckId);
        Debug.Log("removed deck");
    }

    private void MakeDeck()
    {
        DeckTemplates.AddDeck(m_testDeckId, -1);
        Debug.Log("Made Deck");
    }

    private void GiveCardModifiers(CardInDeckStateMachine card)
    {
        IParamModifier<int> setEnergy = new SetValueModifier<int>(23);
        card.SetEnergyOverride(setEnergy);
        Debug.Log($"Gave modifiers to card: {card.GetHashCode()}");
    }

    private void GiveCardSecondModifiers(CardInDeckStateMachine card)
    {
         IParamModifier<int> setEnergy = new SetValueModifier<int>(25);
         card.SetEnergyOverride(setEnergy);       
         Debug.Log($"Gave modifiers to card: {card.GetHashCode()}");
    }

    private void LogDeck()
    {
        Debug.Log($"deck: {m_deck.clientID}");

        foreach (CardInDeckStateMachine card in m_deck.CardsInDeck)
        {
            Debug.Log($"Card: {card.GetCardName()}.. energy: {card.CurrentState.GetEnergy()}");
        }
    }

}
