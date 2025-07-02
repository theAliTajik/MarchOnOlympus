using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public static class DeckTemplates
{
    [System.Serializable]
    public class Deck
    {
        public string clientID;
        public List<CardInDeckStateMachine> CardsInDeck = new List<CardInDeckStateMachine>();
        // public List<BaseCardData> cards = new List<BaseCardData>();
        public bool ReadOnly;
    }

    public static Action OnDecksChanged;
    public static Action OnDeckChanged;
    
    public static List<Deck> Decks = new List<Deck>();

    public static void LoadAllDecks()
    {
        Decks.Clear();

        if (!DeckTemplatesDb.Instance)
        {
            Debug.Log("null predefined deck templates");
        }
        
        
        
        DeckSaveWrapper wrapper = new DeckSaveWrapper();
        wrapper = JsonHelper.LoadAdvanced<DeckSaveWrapper>(Application.persistentDataPath + m_savePath);
        if (wrapper != null)
        {
            Decks.AddRange(wrapper.m_decks);
        }
        
        UpdateDecks();
    }

    public static List<Deck> LoadPredefinedDecks()
    {
        List<Deck> decks = new List<Deck>();
        
        foreach (DeckTemplatesDb.PredefinedDeck predefinedDeck in DeckTemplatesDb.Instance.PredefinedDecks)
        {
            Deck newDeck = new Deck();
            newDeck.clientID = predefinedDeck.ClientId;
            // newDeck.ReadOnly = true;
            foreach (BaseCardData cardData in predefinedDeck.Cards)
            {
                CardInDeckStateMachine card = new CardInDeckStateMachine();
                card.Configure(cardData);
                newDeck.CardsInDeck.Add(card);
            }
            
            decks.Add(newDeck);
        }

        foreach (Deck deck in decks)
        {
            deck.clientID = MakeIDUnique(deck.clientID);
            Decks.Add(deck);
        }
        
        UpdateDecks();
        Save();

        return decks;
    }

    private static string MakeIDUnique(string deckName)
    {
        if (string.IsNullOrEmpty(deckName))
        {
            Debug.Log("ERROR: Null string passed as deck name");
            return deckName;
        }

        int increment = 0;
        while (Decks.Exists(d => d.clientID == deckName))
        {
            deckName = deckName.Replace(increment.ToString(), "");
            increment++;
            deckName += increment.ToString();
        }
        
        return deckName;
    }
    
    public static Deck FindById(string clientId)
    {
        if (string.IsNullOrEmpty(clientId))
        {
            return null;
        }
        
        clientId = clientId.ToLower();
        for (int i = 0; i < Decks.Count; i++)
        {
            Deck template = Decks[i];
            if (template.clientID.ToLower() == clientId)
            {
                return template;
            }
        }

        return null;
    }

    public static bool AddDeck(string deckName, int index)
    {
        if (FindById(deckName) != null)
        {
            Debug.Log("deck name already exists");
            return false;
        }
        
        Deck newDeck = new Deck();
        newDeck.clientID = deckName;

        if (index < 0)
        {
            index = Decks.Count;
        }
        
        Decks.Insert(index, newDeck);
        
        Save();
        UpdateDecks();
        return true;
    }

    public static void RemoveDeck(string deckName)
    {
        if (FindById(deckName) == null)
        {
            Debug.Log("tried to remove non existing deck");
            return;
        }
        
        Decks.Remove(FindById(deckName));
        Save();
        UpdateDecks();
    }
    

    public static void AddCardToDeck(string deckId, string cardId, int index)
    {
        BaseCardData card = CardsDb.Instance.FindById(cardId)?.CardData;
        AddCardToDeck(deckId, card, index);
    }

    public static void AddCardToDeck(string deckId, BaseCardData cardData, int index)
    {
        Deck CurrentDeck = FindById(deckId);
        
        if (CurrentDeck == null)
        {
            Debug.Log("deck null");
            return;
        }
                
        if (cardData == null)
        {
            Debug.Log("card null");
            return;
        }
        
        if (index < 0)
        {
            index = CurrentDeck.CardsInDeck.Count;
        }

        CardInDeckStateMachine newCard = new CardInDeckStateMachine();
        newCard.Configure(cardData);
        CurrentDeck.CardsInDeck.Insert(index, newCard);
                
        Save();
        UpdateDeck();
    } 

    public static void AddCardToDeck(string deckId, CardInDeckStateMachine card, int index)
    {
        
        Deck CurrentDeck = FindById(deckId);
        
        if (CurrentDeck == null)
        {
            Debug.Log("deck null");
            return;
        }
                
        if (card == null)
        {
            Debug.Log("card null");
            return;
        }
        
        if (index < 0)
        {
            index = CurrentDeck.CardsInDeck.Count;
        }

        CurrentDeck.CardsInDeck.Insert(index, card);
                
        Save();
        UpdateDeck();
    }
    
    public static void RemoveCardFromDeck(string deckId, int cardIndex)
    {
        Deck CurrentDeck = FindById(deckId);

        if (cardIndex >= 0 && cardIndex < CurrentDeck.CardsInDeck.Count)
        {
            CurrentDeck.CardsInDeck.RemoveAt(cardIndex);
        }
        
        Save();
        UpdateDeck();
    }

    public static void RemoveCardFromDeck(string deckId, BaseCardData cardData)
    {
        Deck CurrentDeck = FindById(deckId);

        CardInDeckStateMachine card = null;
        for (int i = 0; i < CurrentDeck.CardsInDeck.Count; i++)
        {
            if (CurrentDeck.CardsInDeck[i].GetCardName() == cardData.Name)
            {
                card = CurrentDeck.CardsInDeck[i];
            }
        }
        if (card != null)
        {
            CurrentDeck.CardsInDeck.Remove(card);
        }
        Save();
        UpdateDeck();
    }
    
    
    public static void RemoveCardFromDeck(string deckId, CardInDeckStateMachine cardInDeck)
    {
        
        Deck CurrentDeck = FindById(deckId);

        CardInDeckStateMachine card = null;
        for (int i = 0; i < CurrentDeck.CardsInDeck.Count; i++)
        {
            if (CurrentDeck.CardsInDeck[i] == cardInDeck)
            {
                card = CurrentDeck.CardsInDeck[i];
            }
        }
        if (card != null)
        {
            CurrentDeck.CardsInDeck.Remove(card);
        }

        if (card == null)
        {
            Debug.Log("WARNING: Card removal failed: did not find card in current deck");
        }
        Save();
        UpdateDeck();
    }


    public static void AddCardToDeck(string deckId, BaseCardData cardData)
    {
        Deck CurrentDeck = FindById(deckId);

        CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
        cardInDeck.Configure(cardData);
        CurrentDeck.CardsInDeck.Add(cardInDeck);
        Save();
        UpdateDeck();
    }
    
    [System.Serializable]
    public class DeckSaveWrapper
    {
        public DeckSaveWrapper(List<Deck> decks)
        {
            m_decks.AddRange(decks);
        }

        public DeckSaveWrapper()
        {
            
        }
        
        public List<DeckTemplates.Deck> m_decks = new List<DeckTemplates.Deck>();
    }
    
    private const string m_savePath = "/DeckBuilder/Decks.txt";

    public static void Save()
    {
        DeckSaveWrapper wrapper = new DeckSaveWrapper(Decks);
        
        JsonHelper.SaveAdvanced(wrapper, Application.persistentDataPath + m_savePath); // adjust name/path as needed
    }

    private static void UpdateDecks()
    {
        OnDecksChanged?.Invoke();
    }

    private static void UpdateDeck()
    {
        OnDeckChanged?.Invoke();
    }

}

