using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckBuilderView : MonoBehaviour
{
    public Action<string> OnDeckSelected;
    public Action<string, int> OnDeckAdded;
    public Action<string, string, int > OnDeckDuplicated; // params: original name, duplicate name, index
    public Action<string> OnDeckRemoved;

    public event Action OnLoadDecks;
    
    public Action<string, int> OnCardAddedToDeck;
    public Action<int> OnCardRemovedFromDeck;
    
    
    
    [SerializeField] private PoolDeckItem m_poolDeckItem;
    
    [FormerlySerializedAs("m_DeckContainer")] [SerializeField] private GameObject m_deckContainer;
    [FormerlySerializedAs("m_DeckCardsContainer")] [SerializeField] private GameObject m_deckCardsContainer;
    [FormerlySerializedAs("m_AllCardsContainer")] [SerializeField] private GameObject m_allCardsContainer;

    [SerializeField] private GameObject m_deckCreationPopup;
    [SerializeField] private TMP_InputField m_deckNameInput;
    [SerializeField] private TMP_InputField m_searchInput;
    [SerializeField] private TMP_Text m_deckCreationErrorText;
    [SerializeField] private TMP_Text m_deckNameText;


    private List<DeckItem> m_cardsInDeck = new List<DeckItem>();
    
    
    private List<DeckItem> m_deckItems = new List<DeckItem>();

    public void Start()
    {
        m_deckCreationPopup.SetActive(false);
        m_searchInput.onValueChanged.AddListener(SearchKeyChanged);

        m_deckNameText.text = "";
    }

    private void SearchKeyChanged(string key)
    {
        List<DeckItem> allCards = GetAllChildrenInContainer(m_allCardsContainer);
        if (string.IsNullOrEmpty(key))
        {
            for (int i = 0; i < allCards.Count; i++)
            {
                allCards[i].gameObject.SetActive(true);
            }
        }
        else
        {
            key = key.ToLower();
            for (int i = 0; i < allCards.Count; i++)
            {
                bool matches = allCards[i].ID.ToLower().Contains(key);
                allCards[i].gameObject.SetActive(matches);
            }
        }
    }

    private void OnDestroy()
    {
        List<DeckItem> deckItems = new List<DeckItem>();
        deckItems.AddRange(GetAllChildrenInContainer(m_deckContainer));
        deckItems.AddRange(GetAllChildrenInContainer(m_deckCardsContainer));
        deckItems.AddRange(GetAllChildrenInContainer(m_allCardsContainer));
     
        UnsubscribeDeckItems(deckItems);
    }

    private void UnsubscribeDeckItems(List<DeckItem> deckItems)
    {
        for (var i = 0; i < deckItems.Count; i++)
        {
            UnsubscribeDeckItem(deckItems[i]);
        }
    }

    public void DisplayDecks(List<DeckTemplates.Deck> decks)
    {
        if (decks == null)
        {
            return;
        }
        
        ReturnAllChildrenToPool(m_deckContainer);
        
        for (int i = 0; i < decks.Count; i++)
        {
            DeckItem deckItem = m_poolDeckItem.GetItem();
            deckItem.Reset()
                .EnableDuplicateDeck()
                .SetID(decks[i].clientID)
                .SetIndex(i);

            if (!decks[i].ReadOnly)
            {
                deckItem.EnableRemove();
                deckItem.OnRemoveClicked  += RemoveDeck;
            }
            
            deckItem.OnClicked += SelectDeck;
            deckItem.OnDuplicateClicked += DuplicateDeck;
            
            m_deckItems.Add(deckItem);
            deckItem.transform.SetParent(m_deckContainer.transform, false);
        }
    }

    
    private void SelectDeck(DeckItem deckItem)
    {
        m_deckNameText.text = deckItem.ID;
        OnDeckSelected?.Invoke(deckItem.ID);
    }


    public void DisplayCardsInDeck(DeckTemplates.Deck deck)
    {
        ReturnAllChildrenToPool(m_deckCardsContainer);

        if (deck == null)
        {
            return;
        }

        for (int i = 0; i < deck.CardsInDeck.Count; i++)
        {
            DeckItem deckItem = m_poolDeckItem.GetItem();
            deckItem.Reset()
                .SetID(deck.CardsInDeck[i].GetCardName())
                .SetIndex(i);

            if (!deck.ReadOnly)
            {
                deckItem.EnableRemove()
                    .EnableDuplicate();
                
                deckItem.OnRemoveClicked += RemoveCardFromDeck;
                deckItem.OnDuplicateClicked += DuplicateCardFromDeck;
            }
            
            
            m_deckItems.Add(deckItem);
            deckItem.transform.SetParent(m_deckCardsContainer.transform, false);
            
        }
    
    }

    private void AddCardToDeck(DeckItem deckItem)
    {
        OnCardAddedToDeck?.Invoke(deckItem.ID, -1);
    }
    
    
    private void DuplicateCardFromDeck(DeckItem deckItem)
    {
        OnCardAddedToDeck?.Invoke(deckItem.ID, deckItem.Index + 1);
    }
    
    private void RemoveCardFromDeck(DeckItem deckItem)
    {
        deckItem.OnRemoveClicked -= RemoveCardFromDeck;
        OnCardRemovedFromDeck?.Invoke(deckItem.Index);
    }
    
    
    public void DisplayAllCards(List<BaseCardData> allCards)
    {
        ReturnAllChildrenToPool(m_allCardsContainer);
        
        for (int i = 0; i < allCards.Count; i++)
        {
            DeckItem deckItem = m_poolDeckItem.GetItem();
            deckItem.Reset()
                .EnableAdd()
                .SetID(allCards[i].Name);

            deckItem.OnAddClicked += AddCardToDeck;
            
            deckItem.transform.SetParent(m_allCardsContainer.transform, false);
            
        }
    
    }

    private List<DeckItem> GetAllChildrenInContainer(GameObject container)
    {
        if (container == null)
        {
            Debug.Log("container was null");
        }
        List<DeckItem> children = container.GetComponentsInChildren<DeckItem>(true).ToList();
        if (children == null || children.Count == 0)
        {
            Debug.Log("children count 0 or null");
        }
        return children;
    }

    private void ReturnAllChildrenToPool(GameObject container)
    {
        List<DeckItem> children = GetAllChildrenInContainer(container);
        ReturnAllChildrenToPool(children);
    }
    
    private void ReturnAllChildrenToPool(List<DeckItem> children)
    {
        for (var i = children.Count - 1; i >= 0; i--)
        {
            if (children[i] == null)
            {
                Debug.Log("child was null");
            }
            UnsubscribeDeckItem(children[i]);
            m_poolDeckItem.ReturnToPool(children[i]);
        }
    }

    private void UnsubscribeDeckItem(DeckItem deckItem)
    {
        deckItem.OnRemoveClicked -= RemoveCardFromDeck;
        deckItem.OnDuplicateClicked -= DuplicateCardFromDeck;
    }

    public void OnLoadDecksButtonClicked()
    {
        OnLoadDecks?.Invoke();
    }

    public void OnCreateDeckMenuClicked()
    {
        // turn on pop up 
        m_deckCreationPopup.SetActive(true);
        m_deckCreationErrorText.gameObject.SetActive(false);
    }

    public void CreateDeck()
    {
        string deckName = m_deckNameInput.text;
        OnDeckAdded?.Invoke(deckName, -1);
    }

    public void DeckAddFailed()
    {
        m_deckCreationErrorText.gameObject.SetActive(true);
    }

    public void DeckAddSuccess()
    {
        m_deckCreationPopup.SetActive(false);
    }
    
    private void DuplicateDeck(DeckItem deckItem)
    {
        string deckName = deckItem.ID;

        int i = 1;
        
        string uniqueDeckName = deckName + " (Copy)";
        while (DeckTemplates.FindById(uniqueDeckName) != null)
        { 
            uniqueDeckName = deckName + " (Copy" + i + ")";
            i++;
        }
        OnDeckDuplicated?.Invoke(deckName, uniqueDeckName, deckItem.Index + 1);
    }


    private void RemoveDeck(DeckItem deckItem)
    {
        OnDeckRemoved?.Invoke(deckItem.ID);
    }

}
