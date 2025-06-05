using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class KillingHardlyCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        KillingHardlyCard c = (KillingHardlyCard)cardData;
        int bleedAmount = MechanicsManager.Instance.GetMechanicsStack(target, MechanicType.BLEED);
        if (bleedAmount > 0)
        {
            target.TakeDamage(bleedAmount, CombatManager.Instance.Player, true);
        }

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new BleedMechanic(c.StanceBleed, target));
        }
        
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {

        finishCallback?.Invoke();
        yield break;
    }

}