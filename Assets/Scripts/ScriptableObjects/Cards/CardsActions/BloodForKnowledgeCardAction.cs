using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BloodForKnowledgeCardAction : BaseCardAction
{
    private BloodForKnowledgeCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BloodForKnowledgeCard)cardData;
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.Damage);

        int bleedStack = GameInfoHelper.GetMechanicStack(target, MechanicType.BLEED);

        if (bleedStack > 0)
        {
            GameActionHelper.GainInvent(m_data.InventGain, GainedByCard:true);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}