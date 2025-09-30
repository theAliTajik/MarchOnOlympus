using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;

public class BurningBashCardAction : BaseCardAction
{
    private BurningBashCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BurningBashCard)cardData;
        int burnStack = GameInfoHelper.GetMechanicStack(target, MechanicType.BURN);
        
        GameActionHelper.ReduceMechanicStack(target, burnStack, MechanicType.BURN);

        int damage = burnStack * m_data.BurnMultiplier;
        
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}