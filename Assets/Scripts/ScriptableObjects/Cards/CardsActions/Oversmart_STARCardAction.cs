using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class Oversmart_STARCardAction : BaseCardAction
{
    private Oversmart_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Oversmart_STARCard)cardData;

        bool isAttacking = GameInfoHelper.CheckIfAnyEnemyIsAttackingThisTurn();

        if (isAttacking)
        {
            GameActionHelper.AddMechanicToPlayer(m_data.Fortified, MechanicType.FORTIFIED);
            GameActionHelper.AddMechanicToPlayer(m_data.Strength, MechanicType.STRENGTH);
            GameActionHelper.HealPlayer(m_data.Restore);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}