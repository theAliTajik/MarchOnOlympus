using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiercingArrow_STARCardAction : BaseCardAction
{
    private PiercingArrow_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (PiercingArrow_STARCard)cardData;
        GameActionHelper.AddMechanicToFighter(target, m_data.Bleed, MechanicType.BLEED);
        GameActionHelper.AddMechanicToFighter(target, stack:1, MechanicType.VULNERABLE);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}