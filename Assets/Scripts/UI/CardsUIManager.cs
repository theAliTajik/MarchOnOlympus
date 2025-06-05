using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using MoverExtentions;
using UnityEngine.Serialization;


public class CardsUIManager : Singleton<CardsUIManager>
{
    public event Action<CardDisplay, Fighter> OnCardPick;
    public BaseEnemy testEnemy;
    [SerializeField] private Camera m_cam;
    [SerializeField] private RectTransform m_canvas;
    [SerializeField] private CombatManager m_combatManager;
    [SerializeField] private DynamicArrow m_arrow;
    [SerializeField] private HitBoxesManager m_hitBoxesManager;
    [SerializeField] private CardDisplayList m_cardDisplayList;
    
    
    [SerializeField] private RectTransform m_cardsParent;

    
    [Header("Appearance")]
    [SerializeField] private float m_animationDuration = 0.25f;
    [SerializeField] private Transform m_cardsDrawPileIcon; 
    [SerializeField] private Transform m_cardsDiscardPileIcon;
    [SerializeField] private Vector2 m_cardPlayingPosition;
    [SerializeField] private CardsPositionManager m_cardsPositions;

    private List<CardDisplay> m_cardsInHand = new List<CardDisplay>();
    private TargetType m_currentCardTargetType;

    private float m_totalWidth;
    private float m_cardSpacing;
    private float m_cardStartX;
    
    private CardDisplay m_selectedCard;
    private RainbowWidget m_rainbowWidget;
    //private bool m_isDraggingCard;
    //private float m_startY;
    


    private void Start()
    {
        m_arrow.OnEnemySelected += OnCardPlay;
        m_arrow.OnNoEnemySelected += OnCardNotPlayed;
        GameplayEvents.StanceChanged += OnStanceChanged;

        GameplayEvents.CardPlayFinished += OnCardFinishedPlay;

        GameplayEvents.ShowCards += m_cardDisplayList.ShowCards;

        m_rainbowWidget = RefsHolder.Get<RainbowWidget>();
    }

    protected override void Init()
    {
        
    }

    private void OnDestroy()
    {
        m_arrow.OnEnemySelected -= OnCardPlay;
        m_arrow.OnNoEnemySelected -= OnCardNotPlayed;
        GameplayEvents.StanceChanged -= OnStanceChanged;

        GameplayEvents.CardPlayFinished -= OnCardFinishedPlay;
    }

   
    public void OnStanceChanged(Stance stance)
    {
        if (m_cardsInHand == null)
        {
            Debug.LogError("no cards display");
        }
        foreach (CardDisplay cardDisplay in m_cardsInHand)
        {
            cardDisplay.SetStance(stance);
        } 
    }

    public void EndGame()
    {
        m_rainbowWidget.HideAll();
        m_selectedCard = null;
        StartCoroutine(DiscradAllCardsInHand());
    }

    public void AddCard(CardDisplay cardDisplay)
    {
        if (cardDisplay == null)
        {
            Debug.LogError("null card");
            return;
        }
        cardDisplay.OnDragEnd += OnEndDrag;
        cardDisplay.OnPointerEntered += OnPointerEnter;
        cardDisplay.OnPointerExited += OnPointerExit;
        cardDisplay.OnPlay += OnCardSelected;
        cardDisplay.OnDeselect += OnCardDeselected;
    }
    
    public IEnumerator DisplayCurrentHand(List<CardDisplay> cardsToDraw)
    {
        m_cardsInHand.Clear();
        m_cardsInHand.AddRange(cardsToDraw);
        m_cardsPositions.SetCardsList(m_cardsInHand);
        for (int i = 0; i < m_cardsInHand.Count; i++)
        {
            m_cardsInHand[i].RefreshUI();
            PutCardInPlace(m_cardsInHand[i]);
            yield return new WaitForSeconds(0.1f);
        }
        GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_DRAW_FINISHED);
    }

    public void DisplayCard(CardDisplay cardDisplay)
    {
        m_cardsInHand.Add(cardDisplay);
        m_cardsPositions.AddCardToList(cardDisplay);
        //PutCardInPlace(cardDisplay);
    }
    
    

    
    private void OnPointerEnter(CardDisplay card)
    {
        if (m_selectedCard != null)
        {
            return;
        }

        int index = m_cardsInHand.IndexOf(card);
        m_cardsPositions.SetSelectedIndex(index);
        SetSiblingIndexes(index);
        m_rainbowWidget.Show(card.CardInDeck.CurrentState.GetTargetingType(), 1);
    }

    private void OnPointerExit(CardDisplay card)
    {
        if (m_selectedCard != null)
        {
            return;
        }

        m_rainbowWidget.HideAll();
        SetSiblingIndexes(-1);
        m_cardsPositions.SetSelectedIndex(-1);
    }

    private void OnEndDrag(CardDisplay card)
    {
        if (m_selectedCard != null)
        {
            m_selectedCard.SetState(CardState.IDLE);
            //PutCardInPlace(m_selectedCard);
            m_selectedCard = null;
        }

        m_rainbowWidget.HideAll();
        SetSiblingIndexes(-1);
        m_cardsPositions.SetSelectedIndex(-1);
    }

    private void OnCardDeselected(CardDisplay card)
    {
    }

    private void OnCardSelected(CardDisplay card)
    {
        if (!card.ManaIsEnough)
        {
            //Debug.Log("TODO: not enough mana dialouge");
            //PutCardInPlace(card);
            return;
        }
        
        m_selectedCard = card;
        m_currentCardTargetType = card.CardInDeck.CurrentState.GetTargetingType();
        switch (m_currentCardTargetType)
        {
            case TargetType.ENEMY:
                m_arrow.StartTracking(m_selectedCard, true);
                
                break;
            case TargetType.PLAYER:
                //TODO: highlight player hit box
                m_arrow.StartTracking(m_selectedCard, false);
                break;
            case TargetType.PLAYER_ENEMY:
                //TODO: highlight all hit boxes
                m_arrow.StartTracking(m_selectedCard, false);
                //m_arrow.StopTracking();
                break;
            default:
                Debug.LogError("unknown target type");
                break;
        }

        m_rainbowWidget.Show(m_currentCardTargetType, 2);

    }

    public void OnCardPlay(CardDisplay card, Fighter target)
    {
        OnCardPick?.Invoke(card, target);
        card.SetState(CardState.DEACTIVE);
        card.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        RemoveCard(card);

        card.transform.SetSiblingIndex(0);
        card.MoveTo(m_cardPlayingPosition, m_animationDuration);

        m_selectedCard = null;
        m_rainbowWidget.Show(m_currentCardTargetType, 3);
    }

    private void OnCardFinishedPlay(CardDisplay cardDisplay)
    {
        DiscardCard(cardDisplay);

    }

    private void OnCardNotPlayed()
    {
        m_selectedCard.SetState(CardState.IDLE);
        //PutCardInPlace(m_selectedCard);
        m_selectedCard = null;
        m_cardsPositions.SetSelectedIndex(-1);
        m_rainbowWidget.Show(m_currentCardTargetType, 0);

        SetSiblingIndexes(-1);
    }
    
    public void CalculateWidth()
    {
        m_totalWidth = m_cardsParent.rect.width;
        m_cardStartX = m_totalWidth / -4f;
    }

    private void PutCardInPlace(CardDisplay card)
    {
        m_cardSpacing = m_totalWidth / (m_cardsInHand.Count * 2 - 1);
        
        int cardIndex = m_cardsInHand.IndexOf(card);
        if (cardIndex == -1)
        {
            Debug.LogWarning("Display Card not found in the list");
            return;
        }
        
        Vector2 v = new Vector2(m_cardStartX + cardIndex * m_cardSpacing, 0);
        card.MoveTo(v, m_animationDuration);
        card.transform.SetSiblingIndex(cardIndex);
    }

    private void OnCardPutInPlaceComplete(CardDisplay card)
    {
        Debug.Log(card.CardInDeck.GetCardName() + "put in place");
        card.SetState(CardState.IDLE);
    }

    public IEnumerator OnEndTurn()
    {
        yield return StartCoroutine(DiscradAllCardsInHand()); // wait for discarding of cards
    }

    private IEnumerator DiscradAllCardsInHand()
    {
        while (m_cardsInHand.Count > 0)
        { 
            DiscardCard(m_cardsInHand[0]);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void DiscardCard(CardDisplay card)
    {
        RemoveCard(card);
        card.SetState(CardState.DEACTIVE);
        //card.MoveTo(m_cardPlayingPosition, m_animationDuration);
        
        card.transform.DoCurveMove(m_cardsDiscardPileIcon, 0.2f, null);
        card.transform.DOScale(Vector3.zero, 0.5f);
    }

    public void MoveCardsFromDicardToDrawPile(List<CardDisplay> CardsInDicard, Action OnFinish)
    {
        
    }

    private void MoveCardsFromDicardToDrawPile(CardDisplay card, Action OnFinish)
    {
        card.transform.DOMove(m_cardsDrawPileIcon.position, 0.1f).OnComplete(() =>
        { 
            OnFinish?.Invoke();
        });
    }

    public void RemoveCard(CardDisplay card)
    {
        if (m_cardsInHand.Contains(card))
        {
            m_cardsInHand.Remove(card);
        }
        m_cardsPositions.SetSelectedIndex(-1);
    }
        

    private void SetSiblingIndexes(int selected)
    {
        if (selected == -1)
        {
            for (int i = 0; i < m_cardsInHand.Count; i++)
            {
                m_cardsInHand[i].transform.SetSiblingIndex(i);
            }
        }
        else
        {

            m_cardsInHand[selected].transform.SetSiblingIndex(100);
            return;
            int order = 0;
            int count = m_cardsInHand.Count;
            for (int i = 0; i < m_cardsInHand.Count; i++)
            {
                if (order == 0)
                {
                    m_cardsInHand[selected].transform.SetSiblingIndex(count);
                    order++;
                }
                else
                {
                    if (IsValidIndex(selected + i))
                    {
                        m_cardsInHand[selected + i].transform.SetSiblingIndex(count - order);
                        order++;
                    }

                    if (IsValidIndex(selected - i))
                    {
                        m_cardsInHand[selected - i].transform.SetSiblingIndex(count - order);
                        order++;
                    }
                }
            }
        }

        
    }

    private bool IsValidIndex(int index)
    {
        return (index >= 0 && index < m_cardsInHand.Count);
    }
}



public enum NemoEaseMode
{
    Linear,
    CubicIn,
    CubicOut,
    CubicInOut
}