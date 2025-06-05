using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForcefulHitCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        ForcefulHitCard c = (ForcefulHitCard)cardData;
        int strAmount =
            MechanicsManager.Instance.GetMechanicsStack(CombatManager.Instance.Player, MechanicType.STRENGTH);
        
        
        int damage = 0;
        int baseDamage = (CombatManager.Instance.CurrentStance == cardData.MStance) ? c.StanceDamage : c.Damage;
        int multiplier = (CombatManager.Instance.CurrentStance == cardData.MStance) ? c.StanceStrMultiplier : c.StrMultiplier;

        if (strAmount > 0)
        {
            damage = baseDamage + (multiplier * strAmount) - strAmount;
            target.TakeDamage(damage, CombatManager.Instance.Player, true);
        }
        else
        {
            target.TakeDamage(baseDamage, CombatManager.Instance.Player, true);
        }


        finishCallback?.Invoke();
        yield break;
    }

}