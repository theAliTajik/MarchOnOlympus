using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        if (CardsDb.Instance == null)
        {
            Debug.LogError("CardsDb is empty or not initialized.");
            return;
        }

        List<PredefinedDeck> deckList = new List<PredefinedDeck>();
        
        int deckIndex = 0;
        List<BaseCardData> allCards = CardsDb.Instance.GetAllImplementedCards();
        for (int i = 0; i < allCards.Count; i += 25)
        {
            List<BaseCardData> cards = allCards
                .Skip(i)
                .Take(25)
                .ToList();
            
            PredefinedDeck newTemplate = new PredefinedDeck()
            {
                ClientId = "Deck_" + deckIndex,
                Cards = cards,
            };
            
            deckList.Add(newTemplate);
            deckIndex++;
        }

        
        PredefinedDecks.AddRange(deckList);
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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