using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArsenalCardAction : BaseCardAction
{
    private ArsenalCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ArsenalCard)cardData;
        int damage = m_data.ExtraDamage + GameInfoHelper.GetStartingDeckSize();
        
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        GameActionHelper.AddMechanicToPlayer(m_data.Improvise, MechanicType.IMPROVISE);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}