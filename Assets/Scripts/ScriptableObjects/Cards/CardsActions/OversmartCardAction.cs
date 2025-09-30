using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class OversmartCardAction : BaseCardAction
{
    private OversmartCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (OversmartCard)cardData;

        bool isAttacking = GameInfoHelper.CheckIfAnyEnemyIsAttackingThisTurn();
        if (isAttacking)
        {
            GameActionHelper.DrawCards(m_data.Draw);
        }
        
        GameActionHelper.AddMechanicToPlayer(m_data.Fortified, MechanicType.FORTIFIED);
        GameActionHelper.GainEnergy(m_data.Energy);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}