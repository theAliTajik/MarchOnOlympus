using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Mono.CSharp;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpikesCardAction : BaseCardAction
{
    private SpikesCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (SpikesCard)cardData;
        
        GameActionHelper.AddMechanicToPlayer(m_data.Block, MechanicType.BLOCK);
        GameActionHelper.AddMechanicToPlayer(m_data.Bleed, MechanicType.BLEED);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}