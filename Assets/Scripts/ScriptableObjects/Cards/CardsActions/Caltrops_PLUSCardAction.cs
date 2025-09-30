using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class Caltrops_PLUSCardAction : BaseCardAction
{
    private Caltrops_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Caltrops_PLUSCard)cardData;
        
        GameActionHelper.AddMechanicToPlayer(m_data.Block, MechanicType.BLOCK);
        GameActionHelper.AddMechanicToPlayer(m_data.Thorns, MechanicType.THORNS);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}