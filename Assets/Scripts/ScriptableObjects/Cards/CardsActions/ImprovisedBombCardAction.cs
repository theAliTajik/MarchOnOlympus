using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovisedBombCardAction : BaseCardAction
{
    private ImprovisedBombCard m_data;
    private Fighter m_target;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ImprovisedBombCard)cardData;
        m_target = target;
        
        PerformAction();
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    public override void Discarded(BaseCardData cardData)
    {
        base.Discarded(cardData);
        PerformAction();
    }

    private void PerformAction()
    {
        GameActionHelper.DamageFighter(m_target, GameInfoHelper.GetPlayer(), m_data.Damage);
        GameActionHelper.GainInvent(m_data.Invent);
        GameActionHelper.AddMechanicToFighter(m_target, m_data.Impale, MechanicType.IMPALE);
    }

}