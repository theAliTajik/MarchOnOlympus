using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverplanCardAction : BaseCardAction
{
    private OverplanCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (OverplanCard)cardData;
        
        GameActionHelper.AddMechanicToFighter(target, m_data.Vulnerable, MechanicType.VULNERABLE);

        int inventLevel = GameInfoHelper.GetInventLevel();

        if (inventLevel > m_data.InventLevelThreshold)
        {
            GameActionHelper.AddMechanicToPlayer(m_data.Fortified, MechanicType.FORTIFIED);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}