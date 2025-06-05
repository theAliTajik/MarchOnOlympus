using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RehearsalCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        RehearsalCard c = (RehearsalCard)cardData;

        int returnToHandAmount = c.CardReturnAmount;
        int reduceCost = c.CostReduction;
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            returnToHandAmount = c.StanceCardReturnAmount;
            reduceCost = c.StanceCostReduction;
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
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.NORMAL, -reduceCost, true);
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.STANCE, -reduceCost, true);
            GameActionHelper.MoveCardToHand(card);
            
        }
        
        
        
        finishCallback?.Invoke();
        yield break;
    }

}