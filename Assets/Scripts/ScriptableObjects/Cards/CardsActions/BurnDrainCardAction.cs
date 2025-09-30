using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BurnDrainCardAction : BaseCardAction
{
    private BurnDrainCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BurnDrainCard)cardData;

        int burnStack = GameInfoHelper.GetMechanicStack(target, MechanicType.BURN);
        
        GameActionHelper.ReduceMechanicStack(target, burnStack, MechanicType.BURN);
        GameActionHelper.AddMechanicToFighter(target, burnStack, MechanicType.BLEED);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}