using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class DeckBuilderPresenter : MonoBehaviour
{
    
    [SerializeField] private DeckBuilderView m_view;
    
    private List<DeckItem> m_Decks;
    private string m_currentDeck;

    private void Start()
    {
        DeckTemplates.OnDecksChanged += OnDecksChanged;
        DeckTemplates.OnDeckChanged += OnDeckChanged;
        DeckTemplates.LoadAllDecks();

        m_view.OnDeckSelected += OnDeckClicked;
        m_view.OnCardRemovedFromDeck += OnDeckCardRemoveClicked;
        m_view.OnCardAddedToDeck += OnCardAddClicked;
        m_view.OnDeckAdded += OnDeckAddClicked;
        m_view.OnDeckRemoved += OnDeckRemoveClicked;
        m_view.OnDeckDuplicated += OnDeckDuplicateClicked;
        

        m_view.DisplayAllCards(CardsDb.Instance.AllCards
            .Where(card => card.IsImplemented)
            .Select(card => card.CardData).ToList());
    }

    private void OnDestroy()
    {
        DeckTemplates.OnDecksChanged -= OnDecksChanged;
        DeckTemplates.OnDeckChanged -= OnDeckChanged;
        
        m_view.OnDeckSelected -= OnDeckClicked;
        m_view.OnCardRemovedFromDeck -= OnDeckCardRemoveClicked;
        m_view.OnCardAddedToDeck -= OnCardAddClicked;
        m_view.OnDeckAdded -= OnDeckAddClicked;
        m_view.OnDeckRemoved -= OnDeckRemoveClicked;
        m_view.OnDeckDuplicated -= OnDeckDuplicateClicked;
    }

    private void OnDecksChanged()
    {
        if (DeckTemplates.FindById(m_currentDeck) == null)
        {
            m_currentDeck = string.Empty;
            m_view.DisplayCardsInDeck(null);
        }
        m_view.DisplayDecks(DeckTemplates.Decks);
    }
    
    private void OnDeckChanged()
    {
        DispalyCardsInDeckCurrentDeck();
    }

    private void OnDeckClicked(string deckId)
    {
        if (m_currentDeck == deckId)
        {
            return;
        }
        m_currentDeck = deckId;

        DispalyCardsInDeckCurrentDeck();
    }

    private void DispalyCardsInDeckCurrentDeck()
    {
        DeckTemplates.Deck deck = DeckTemplates.FindById(m_currentDeck);

        m_view.DisplayCardsInDeck(deck);
    }
    
    private void OnDeckAddClicked(string deckId, int index)
    {
        bool success = DeckTemplates.AddDeck(deckId, index);

        if (success)
        {
            m_view.DeckAddSuccess();
        }
        else
        {
            m_view.DeckAddFailed();
        }
    }

    private void OnDeckDuplicateClicked(string deckId, string duplicateId, int index)
    {
        bool success = DeckTemplates.AddDeck(duplicateId, index);

        if (success)
        {
            DeckTemplates.Deck oldDeck = DeckTemplates.FindById(deckId);
            DeckTemplates.Deck newDeck = DeckTemplates.FindById(duplicateId);
            
            newDeck.CardsInDeck.AddRange(oldDeck.CardsInDeck);
        }
    }

    private void OnDeckRemoveClicked(string deckId)
    {
        DeckTemplates.RemoveDeck(deckId);
    }

    private void OnDeckCardDuplicateClicked(string cardId)
    {
        
    }

    private void OnDeckCardRemoveClicked(int cardIndex)
    {
        if (m_currentDeck == null)
        {
            UnityEngine.Debug.Log("trieed to remove card from null deck");
            return;
        }

        UnityEngine.Debug.Log("remove card at index: " + cardIndex);
        DeckTemplates.RemoveCardFromDeck(m_currentDeck, cardIndex);
    }

    private void OnCardAddClicked(string cardId, int index)
    {
        if (string.IsNullOrEmpty(cardId) || string.IsNullOrEmpty(m_currentDeck))
        {
            UnityEngine.Debug.Log("card id or current deck null");
            return;
        }

        DeckTemplates.Deck deck = DeckTemplates.FindById(m_currentDeck);

        if (deck.ReadOnly)
        {
            UnityEngine.Debug.Log("cannot add to read only deck");
            return;
        }

        cardId = cardId.Replace(" ", "");
        UnityEngine.Debug.Log("card id:" + cardId);
        
        DeckTemplates.AddCardToDeck(m_currentDeck, cardId, index);
        
    }
    
    private List<string> GetCardTemplatesFromScriptableObject()
    {
        List<string> cardsTemplate = new List<string>();
        cardsTemplate.AddRange(DeckTemplatesDb.Instance.allDecks.Select(t => t.clientID));

        if (cardsTemplate.Count <= 0)
        {
            UnityEngine.Debug.Log("no deck templates found in scriptable object");
        }
        return cardsTemplate;
    }

    private List<string> GetAllCardsFromScriptableObject()
    {
        List<string> cards = new List<string>();
        cards.AddRange(CardsDb.Instance.AllCards.Select(t => t.clientID));

        if (cards.Count <= 0)
        {
            UnityEngine.Debug.Log("no cards found in scriptable object");
        }
        return cards;

    }
}
