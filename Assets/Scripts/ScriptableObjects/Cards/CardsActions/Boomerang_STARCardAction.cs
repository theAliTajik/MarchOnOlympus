using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boomerang_STARCardAction : BaseCardAction
{
    private Boomerang_STARCard m_data;
    private int m_numOfTimesPlayed;
    private Fighter m_target;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Boomerang_STARCard)cardData;
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
        int damage = m_data.Damage;
        
        damage += m_data.DamageIncrease * m_numOfTimesPlayed;
        m_numOfTimesPlayed++;
        
        GameActionHelper.DamageFighter(m_target, GameInfoHelper.GetPlayer(), damage);
    }
}