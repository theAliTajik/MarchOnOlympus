using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DeckTemplatesDb", menuName = "Olympus/Deck Templates Db")]
public class DeckTemplatesDb : GenericData<DeckTemplatesDb>
{

    [Serializable]
    public struct PredefinedDeck
    {
        public string ClientId;
        public List<BaseCardData> Cards;
    }
    
    public List<PredefinedDeck> PredefinedDecks;
    
#if UNITY_EDITOR
    [ContextMenu("generate a deck for all cards")]
    public void GenerateDeckForAllCards()
    {
        if (CardsDb.Instance == null || CardsDb.Instance.AllCards == null || CardsDb.Instance.AllCards.Count == 0)
        {
            Debug.LogError("CardsDb is empty or not initialized.");
            return;
        }

        List<DeckTemplates.Deck> deckList = new List<DeckTemplates.Deck>();
        // deckList.AddRange(allDecks);
        
        int deckIndex = 0;
        for (int i = 0; i < CardsDb.Instance.AllCards.Count; i += 25)
        {
            List<BaseCardData> cards = CardsDb.Instance.AllCards
                .Skip(i)
                .Take(25)
                .Select(cardInfo => cardInfo.CardData)
                .ToList();
            List<CardInDeckStateMachine> cardsInDeck = new List<CardInDeckStateMachine>();
            foreach (BaseCardData baseCardData in cards)
            {
                CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
                cardInDeck.Configure(baseCardData);
                cardsInDeck.Add(cardInDeck);
            }
            DeckTemplates.Deck newTemplate = new DeckTemplates.Deck
            {
                clientID = "Deck_" + deckIndex,
                CardsInDeck = cardsInDeck,
            };
            
            deckList.Add(newTemplate);
            deckIndex++;
        }

        // Convert the list back to an array after processing all cards
        // allDecks = deckList.ToArray();
    }

#endif


    public PredefinedDeck? FindById(string clientId)
    {
        for (int i = 0; i < PredefinedDecks.Count; i++)
        {
            PredefinedDeck template = PredefinedDecks[i];
            if (template.ClientId == clientId)
            {
                return template;
            }
        }

        return null;
    }
}