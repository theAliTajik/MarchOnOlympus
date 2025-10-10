using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovisedArrowCardAction : BaseCardAction
{
    private ImprovisedArrowCard m_data;
    private Fighter m_target;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ImprovisedArrowCard)cardData;
        m_target = target;
        DamageAndImpale();
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    public override void Discarded(BaseCardData cardData)
    {
        base.Discarded(cardData);
        DamageAndImpale();
    }

    private void DamageAndImpale()
    {
        if (m_target == null)
        {
            m_target = GameInfoHelper.GetRandomEnemy();
        }

        if (m_target == null)
        {
            CustomDebug.LogError("Could not Get target for improvised arrow", Categories.Combat.Cards);
            return;
        }
        
        GameActionHelper.DamageFighter(m_target, GameInfoHelper.GetPlayer(), m_data.Damage);
        GameActionHelper.AddMechanicToFighter(m_target, m_data.Impale, MechanicType.IMPALE);
    }

}