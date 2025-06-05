using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class BlockStrikeCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        BlockStrikeCard c = (BlockStrikeCard)cardData;
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
            CombatManager.Instance.ForceChangeStance(Stance.BATTLE);
        }
        else
        {
            BaseMechanic b = new BlockMechanic(c.Block, CombatManager.Instance.Player);
            MechanicsManager.Instance.AddMechanic(b);    
        }
        
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {

        finishCallback?.Invoke();
        yield break;
    }
    
}
