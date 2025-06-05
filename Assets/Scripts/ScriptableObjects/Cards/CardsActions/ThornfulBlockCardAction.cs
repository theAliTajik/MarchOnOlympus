using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class ThornfulBlockCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        ThornfulBlockCard c = (ThornfulBlockCard)cardData;
        
        MechanicsManager.Instance.AddMechanic(new BlockMechanic(c.Block, CombatManager.Instance.Player));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            target.TakeDamage(c.StanceDamage, CombatManager.Instance.Player, true);
        }
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {

        finishCallback?.Invoke();
        yield break;
    }
    
}