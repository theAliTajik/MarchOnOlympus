using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PonderCardAction : BaseCardAction
{
    private List<CardDisplay> m_changedCards = new List<CardDisplay>();
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        PonderCard c = (PonderCard)cardData;
        
        //draw cards 
        m_changedCards = GameActionHelper.DrawCards(c.CardDrawAmount);
        //override cost
        foreach (CardDisplay card in m_changedCards)
        {
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.NORMAL, c.CardCost);
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.STANCE, c.CardCost);
        }
        // remove override next turn
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
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
            foreach (CardDisplay card in m_changedCards)
            {
                card.CardInDeck.RemoveEnergyOverride();
            }
        }
    }
}