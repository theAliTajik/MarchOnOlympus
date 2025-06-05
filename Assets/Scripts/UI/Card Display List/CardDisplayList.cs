using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardDisplayList : MonoBehaviour
{
    public event Action<CardDisplay> ItemClicked;
    public event Action OnClosed;


    [SerializeField] private Transform m_itemsParent;
    [SerializeField] private float m_cardScale;
    
    private bool m_didSelectCard = false;
    
    private List<CardDisplay> m_cards = new List<CardDisplay>();


    private void OnDestroy()
    {
        GameplayEvents.ShowCards -= ShowCards;
        GameplayEvents.ShowCardsByData -= ShowCards;
    }

    public void ShowCards(CardPile cardPile)
    {
        AddCards(cardPile);
        gameObject.SetActive(true);
        OnEnable();
    }

    public void ShowCards(List<CardInDeckStateMachine> cardDatas)
    {
        Debug.Log("show cards command entered");
        AddCards(cardDatas);
        gameObject.SetActive(true);
        OnEnable();
    }

    public void AddCards(List<CardInDeckStateMachine> cardDatas)
    {
        ReturnToPool();

        foreach (CardInDeckStateMachine cardData in cardDatas)
        {
            CardDisplay cd = PoolCardDisplay.Instance.GetItem();
            cd.Configure(cardData);
            AddCard(cd);
        }
    } 
    
    public virtual void AddCards(CardPile cardPile)
    {
        ReturnToPool();


        for (var i = 0; i < cardPile.Cards.Count; i++)
        {
           AddCard(cardPile.Cards[i]);
           
        }
    }

    private void ReturnToPool()
    {
        for (int i = 0; i < m_cards.Count; i++)
        {
            PoolCardDisplay.Instance.ReturnToPool(m_cards[i]);
        }
        m_cards.Clear();
    }
    private CardDisplay AddCard(CardDisplay cardData)
    {
        //CardClickableItem card = PoolCardClickableItem.Instance.GetItem();
        CardDisplay card = PoolCardDisplay.Instance.GetItem();
        
        card.transform.SetParent(m_itemsParent, false);
        card.transform.localScale = Vector3.one * m_cardScale;
        card.Configure(cardData.CardInDeck);
        card.RefreshUI();
        
        card.OnClick += OnItemClicked;
        
        m_cards.Add(card);
        return card;
    }

    protected virtual void OnItemClicked(CardDisplay card)
    {
        Close();
        ItemClicked?.Invoke(card);
        Debug.Log("card clicked: " + card.CardInDeck.GetCardName());
        GameplayEvents.SendCardSelectedByPlayer(card);
        m_didSelectCard = true;
    }

    protected virtual void OnEnable()
    {
        m_itemsParent.localScale = Vector3.zero;
        m_itemsParent.DOScale(Vector3.one, 0.25f);
    }

    public void Close()
    {
        for (var i = 0; i < m_cards.Count; i++)
        {
            PoolCardDisplay.Instance.ReturnToPool(m_cards[i]);
        }
        
        m_cards.Clear();
       
        gameObject.SetActive(false);
        OnClosed?.Invoke();
        
        if (!m_didSelectCard)       
        {
            GameplayEvents.SendCardNotSelected();
        }
    }



}
