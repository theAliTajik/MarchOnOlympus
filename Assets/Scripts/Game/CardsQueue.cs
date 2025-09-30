using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardsQueue : Singleton<CardsQueue>
{
    public struct CardQueueItem
    {
        public Action OnCardPlayFinished;
        public BaseCardAction action;
        public CardDisplay cardDisplay;
        public Fighter target;

        public CardQueueItem(Action OnCardPlayFinished, BaseCardAction action, CardDisplay cardDisplay, Fighter target)
        {
            this.OnCardPlayFinished = OnCardPlayFinished;
            this.action = action;
            this.cardDisplay = cardDisplay;
            this.target = target;
        }
    }
    private Queue<CardQueueItem> m_cardQueue = new Queue<CardQueueItem>();
    
    public struct ExtraActionItem
    {
        public int OwnerID;
        public Action<CardDisplay, Fighter> action;

        public ExtraActionItem(MonoBehaviour owner, Action<CardDisplay, Fighter> action)
        {
            this.OwnerID = owner.GetInstanceID();
            this.action = action;
        }
    }
    private List<ExtraActionItem> m_extraActions = new List<ExtraActionItem>();
    
    private bool m_isProcessing = false;
    private bool m_isAnimating = false;
    
    public bool IsProcessing { get { return m_isProcessing; } }

    public void  AddToQueue(Action OnCardPlayFinished, BaseCardAction action, CardDisplay cardDisplay, Fighter target)
    {
        m_cardQueue.Enqueue(new CardQueueItem(OnCardPlayFinished, action, cardDisplay, target));
        if (!m_isProcessing)
        {
            StartCoroutine(ProssesNextInQueue());
        }
    }

    public void AddExtraAction(MonoBehaviour owner, Action<CardDisplay, Fighter> action)
    {
        m_extraActions.Add(new ExtraActionItem(owner, action));
    }

    public void RemoveExtraAction(MonoBehaviour owner)
    {
        m_extraActions.RemoveAll(x => x.OwnerID == owner.GetInstanceID());
    }
    
    public IEnumerator ProssesNextInQueue()
    {
        m_isProcessing = true;
        GameplayEvents.SendChangeEndTurnButtonState(false);
        
        bool cardFinished = false;

        if (!m_isAnimating)
        {
            m_isAnimating = true;
        }
        float waitTime = CombatManager.Instance.Player.PlayAttackAnimation(() => m_isAnimating = false);
        //yield return new WaitForSeconds(waitTime); // wait until hitting point of animation
        
        CardQueueItem item = m_cardQueue.Dequeue();
        item.action.Play(item.cardDisplay.CardInDeck.GetCardData(), () => cardFinished = true , item.target, item.cardDisplay);
        CustomDebug.Log($"Card Played: {item.cardDisplay.CardInDeck.GetCardName()}", Categories.Combat.Cards);
        GameplayEvents.SendOnCardPlayed(item.cardDisplay);
        
        //play extra actions
        for (int i = 0; i < m_extraActions.Count; i++)
        {
            m_extraActions[i].action?.Invoke(item.cardDisplay, item.target); 
        }
        
        yield return new WaitUntil(() => cardFinished);
        item.OnCardPlayFinished?.Invoke();

        //yield return new WaitUntil(() => playerAnimationFinished);

        if (m_cardQueue.Count > 0)
        {
            StartCoroutine(ProssesNextInQueue());
            yield break;
        }

        m_isProcessing = false;
        if (GameInfoHelper.IsPlayerTurn())
        {
            GameplayEvents.SendChangeEndTurnButtonState(true);
        }
    }

    protected override void Init()
    {
        
    }

}
