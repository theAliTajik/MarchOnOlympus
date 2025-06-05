using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerfectTechniqueCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        PerfectTechniqueCard c = (PerfectTechniqueCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        GameActionHelper.AddMechanicToPlayer(c.BlockGain, MechanicType.BLOCK);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            GameActionHelper.HealPlayer(c.Restore);
        }
        
        //end turn
        GameActionHelper.EndTurn();
        
        finishCallback?.Invoke();
        yield break;
    }

}