using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class KillingThoughtfullyCardAction : BaseCardAction
{
    private KillingThoughtfullyCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (KillingThoughtfullyCard)cardData;
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.Damage);
        
        GameActionHelper.AddMechanicToFighter(target, m_data.Bleed, MechanicType.BLEED);
        GameplayEvents.SendOnGainInvent(m_data.Invent);
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}