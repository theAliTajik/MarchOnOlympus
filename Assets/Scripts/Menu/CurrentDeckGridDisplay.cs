
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrentDeckGridDisplay : MonoBehaviour
{
    [SerializeField] private GameObject m_container;

    private void Awake()
    {
        DisplayCurrentDeck();
        MenuEventBus.OnDeckSelected += DisplayDeck;
    }

    private void DisplayDeck(string deckId)
    {
        Debug.Log($"was asked to display deck: {deckId}");
        ReturnAllChildrenToPool(m_container);

        var deck = DeckTemplates.FindById(deckId);

        if (deck == null)
        {
            Debug.Log("ERROR: null deck");
            return;
        }

        for (int i = 0; i < deck.CardsInDeck.Count; i++)
        {
            CardDisplay card = PoolCardDisplay.Instance.GetItem();
            card.transform.SetParent(m_container.transform, worldPositionStays: false);
            card.transform.localScale = Vector3.one;
            card.Configure(deck.CardsInDeck[i]);
        }
    }

    private void DisplayCurrentDeck()
    {
        string currentDeckID = GameSessionParams.DeckTemplateClientId;
        DisplayDeck(currentDeckID);
    }
    
    private List<CardDisplay> GetAllChildrenInContainer(GameObject container)
    {
        if (container == null)
        {
            Debug.Log("container was null");
        }
        List<CardDisplay> children = container.GetComponentsInChildren<CardDisplay>(true).ToList();
        if (children == null || children.Count == 0)
        {
            Debug.Log("children count 0 or null");
        }
        return children;
    }
    
    private void ReturnAllChildrenToPool(GameObject container)
    {
        List<CardDisplay> children = GetAllChildrenInContainer(container);
        ReturnAllChildrenToPool(children);
    }
    
    private void ReturnAllChildrenToPool(List<CardDisplay> children)
    {
        for (var i = children.Count - 1; i >= 0; i--)
        {
            if (children[i] == null)
            {
                Debug.Log("child was null");
            }
            PoolCardDisplay.Instance.ReturnToPool(children[i]);
        }
    }


    
}
