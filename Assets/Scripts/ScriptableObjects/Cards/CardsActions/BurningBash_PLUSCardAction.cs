using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BurningBash_PLUSCardAction : BaseCardAction
{
    private BurningBash_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BurningBash_PLUSCard)cardData;

        int removedBurn = GameActionHelper.RemoveMechanicOfType(target, MechanicType.BURN);
        int damage = removedBurn * m_data.BurnMultiplier;
        
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}