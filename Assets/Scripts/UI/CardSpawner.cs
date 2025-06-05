using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardSpawner : Singleton<CardSpawner>
{
    
    [SerializeField] private GameObject m_cardPrefab;
    [SerializeField] private Canvas m_canvas;
    
    [SerializeField] private Vector2 cardSpawnPosition = Vector2.zero;
    

    public CardDisplay SpawnCard(BaseCardData card)
    {
        if (m_cardPrefab == null || card == null)
        {
            Debug.LogWarning("Card Prefab or Card ScriptableObject is not assigned in the inspector!");
            return null;
        }
        
        CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
        cardInDeck.Configure(card);
        
        return SpawnCard(cardInDeck);
    }

    public CardDisplay SpawnCard(CardInDeckStateMachine cardInDeck)
    {
        if (cardInDeck == null)
        {
            Debug.Log("ERROR: Card in deck is null");
        }
        
        GameObject spawnedCard = Instantiate(m_cardPrefab, this.transform);
        
        RectTransform rectTransform = spawnedCard.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = cardSpawnPosition;
        
        CardDisplay cardDisplay = spawnedCard.GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            Debug.LogWarning("The instantiated prefab does not have a CardDisplay component!");
            return null;
        }
        cardDisplay.Configure(cardInDeck, m_canvas.scaleFactor);

        //Debug.Log("spawned card: " + cardDisplay.CardInDeck.GetCardName());
        return cardDisplay;
    }

    public CardDisplay SpawnCardByName(string name)
    {
        BaseCardData scriptableObject = Resources.Load<BaseCardData>("CardsData/" + name);
        if (scriptableObject == null)
        {
            Debug.Log($"ScriptableObject with name '{name}' not found in Resources!");
            return null;
        }
        return SpawnCard(scriptableObject);
        
    }

    private GameObject LoadPrefabByName(string prefabName)
    {
        return Resources.Load<GameObject>($"Prefabs/{prefabName}");
    }

    protected override void Init()
    {
        
    }
}
