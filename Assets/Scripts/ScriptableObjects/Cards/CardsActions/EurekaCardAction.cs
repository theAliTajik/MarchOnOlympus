using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EurekaCardAction : BaseCardAction
{
    private EurekaCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (EurekaCard)cardData;
        
        int cardsPlayed = GameInfoHelper.CountNumOfCardsPlayed();
        int nextCardPlayed = cardsPlayed + 1;
        GameActionHelper.SetCardToBePlayedTwice(nextCardPlayed);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}