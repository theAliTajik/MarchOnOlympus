using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReadyCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ReadyCard c = (ReadyCard)cardData;
        
        GameActionHelper.SetCardToBePlayedTwice(CardPacks.IMPALE);
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}