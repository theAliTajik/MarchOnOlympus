using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class PonderInBloodCardAction : BaseCardAction
{
    private PonderInBloodCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (PonderInBloodCard)cardData;
        
        int inventGain = GameInfoHelper.GetMechanicStack(target, MechanicType.BLEED);
        GameplayEvents.SendOnGainInvent(inventGain);
        
        int invent = GameInfoHelper.GetInvent();
        int damage = invent;
        
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}