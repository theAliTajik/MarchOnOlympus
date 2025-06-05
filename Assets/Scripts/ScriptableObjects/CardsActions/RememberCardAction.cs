using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RememberCardAction : BaseCardAction
{
    private List<CardDisplay> m_cardsChanged = new List<CardDisplay>();

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        RememberCard c = (RememberCard)cardData;
        
        int returnToHandAmount = c.CardReturnAmount;
        int cost = c.CardCost;
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            returnToHandAmount = c.StanceCardReturnAmount;
            cost = c.StanceCardCost;
        }
            
        // rand card from dp by amount
        int numOfCardsInDiscardPile = GameInfoHelper.CountNumOfCardsInDeck(CardStorage.DISCARD_PILE);
        if (returnToHandAmount > numOfCardsInDiscardPile)
        {
            returnToHandAmount = numOfCardsInDiscardPile;
        }
        for (int i = 0; i < returnToHandAmount; i++)
        {
            CardDisplay card = GameInfoHelper.GetRandomCard(CardStorage.DISCARD_PILE);
            if (card == null)
            {
                continue;
            }
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.NORMAL, cost);
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.STANCE, cost);
            GameActionHelper.MoveCardToHand(card);
            m_cardsChanged.Add(card);
        }

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;

    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if (phase == EGamePhase.PLAYER_TURN_START)
        {
            for (var i = 0; i < m_cardsChanged.Count; i++)
            {
                m_cardsChanged[i].CardInDeck.NormalState.RemoveAllEnergyOverrides();
                m_cardsChanged[i].CardInDeck.StanceState.RemoveAllEnergyOverrides();
            }
            GameplayEvents.GamePhaseChanged -= OnPhaseChange;
        }
    }

}