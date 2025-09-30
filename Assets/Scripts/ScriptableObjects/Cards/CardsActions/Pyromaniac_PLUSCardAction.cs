using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pyromaniac_PLUSCardAction : BaseCardAction
{
    private Pyromaniac_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Pyromaniac_PLUSCard)cardData;
        
        int burnStack = GameInfoHelper.GetMechanicStack(target, MechanicType.BURN);

        if (burnStack > 0)
        {
            GameActionHelper.HealPlayer(m_data.Restore);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}