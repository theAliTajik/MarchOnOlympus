using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShuffleCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ShuffleCard c = (ShuffleCard)cardData;
        
        // discard rand card from hand
        CardDisplay randCard = GameInfoHelper.GetRandomCard(CardStorage.HAND);
        GameActionHelper.DiscardCard(randCard);
        // card costs 1 less
        GameActionHelper.SetCardEnergyOverride(randCard, ECardInDeckState.NORMAL, -c.CardCostReduction, true);
        GameActionHelper.SetCardEnergyOverride(randCard, ECardInDeckState.STANCE, -c.CardCostReduction, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}