using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CookBookCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        CookBookCard c = (CookBookCard)cardData;
        
        //rand card from starting deck
        int rand = UnityEngine.Random.Range(0, GameplayController.Instance.StartingCards.Count);
        CardInDeckStateMachine randCard = GameplayController.Instance.StartingCards[rand];
        CardDisplay cd = GameActionHelper.SpawnCard(randCard, CardStorage.HAND);
        
        GameActionHelper.SetCardEnergyOverride(cd, ECardInDeckState.NORMAL, c.CardCostOverride);
        GameActionHelper.SetCardEnergyOverride(cd, ECardInDeckState.STANCE, c.CardCostOverride);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}