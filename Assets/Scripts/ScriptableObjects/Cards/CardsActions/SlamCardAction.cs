using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlamCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        SlamCard c = (SlamCard)cardData;
        int damage = MechanicsManager.Instance.GetMechanicsStack(CombatManager.Instance.Player, MechanicType.BLOCK);
        if (damage == -1)
        {
            damage = 0;
        }
        else
        {
            MechanicsManager.Instance.RemoveMechanic(CombatManager.Instance.Player, MechanicType.BLOCK);
            if (CombatManager.Instance.CurrentStance == cardData.MStance)
            {
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(c.block, target));
            }
            target.TakeDamage(damage, CombatManager.Instance.Player, true);
        }
        

       
        
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}