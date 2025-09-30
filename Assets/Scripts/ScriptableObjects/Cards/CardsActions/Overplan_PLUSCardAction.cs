using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Mono.CSharp;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class Overplan_PLUSCardAction : BaseCardAction
{
    private Overplan_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Overplan_PLUSCard)cardData;
        
        GameActionHelper.AddMechanicToFighter(target, m_data.Vulnerable, MechanicType.VULNERABLE);
        GameActionHelper.AddMechanicToPlayer(m_data.Fortified, MechanicType.FORTIFIED);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}