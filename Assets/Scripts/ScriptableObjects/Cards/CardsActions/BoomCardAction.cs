using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoomCardAction : BaseCardAction
{
    private BoomCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BoomCard)cardData;
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.Damage);

        int vulnerableStack = GameInfoHelper.GetMechanicStack(target, MechanicType.VULNERABLE);

        if (vulnerableStack > 0)
        {
            GameActionHelper.AddMechanicToFighter(target, m_data.Burn, MechanicType.BURN);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}