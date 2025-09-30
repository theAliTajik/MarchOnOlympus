using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pyromaniac_STARCardAction : BaseCardAction
{
    private Pyromaniac_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Pyromaniac_STARCard)cardData;

        int burnStack = GameInfoHelper.GetMechanicStack(target, MechanicType.BURN);

        if (burnStack > 0)
        {
            GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.Damage);
        }
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}