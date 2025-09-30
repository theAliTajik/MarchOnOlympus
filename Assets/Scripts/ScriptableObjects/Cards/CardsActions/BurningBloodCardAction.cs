using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BurningBloodCardAction : BaseCardAction
{
    private BurningBloodCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BurningBloodCard)cardData;
        int burnStack = GameInfoHelper.GetMechanicStack(GameInfoHelper.GetPlayer(), MechanicType.BURN);
        int damage = burnStack;
        int bleedAmount = burnStack / 2;
        
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        GameActionHelper.AddMechanicToFighter(target, bleedAmount, MechanicType.BLEED);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}