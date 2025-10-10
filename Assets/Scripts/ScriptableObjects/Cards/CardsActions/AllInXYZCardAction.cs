using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllInXYZCardAction : BaseCardAction
{
    private AllInXYZCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (AllInXYZCard)cardData;
        int numOfCardsInHand = GameInfoHelper.GetNumOfCardsInHand();
        GameActionHelper.DiscardAllCardsInHand();
        int damage = numOfCardsInHand * m_data.DamageMultiplier;
        
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}