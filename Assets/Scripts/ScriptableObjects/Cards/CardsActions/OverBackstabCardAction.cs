using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverBackstabCardAction : BaseCardAction
{
    private OverBackstabCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (OverBackstabCard)cardData;
        
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.Damage);

        int numOfDebuff = GameInfoHelper.GetNumOfDebuffMechanics(target);
        if (numOfDebuff > 0)
        {
            GameActionHelper.GainEnergy(m_data.Energy);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}