using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovisedBomb_STARCardAction : BaseCardAction
{
    private ImprovisedBomb_STARCard m_data;
    private Fighter m_target;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ImprovisedBomb_STARCard)cardData;
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
        GameActionHelper.AddMechanicToPlayer(m_data.Vulnerable, MechanicType.VULNERABLE);
    }
}