using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class KillingSoftlyCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        KillingSoftlyCard c = (KillingSoftlyCard)cardData;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new ImpaleMechanic(c.ImpaleAmount, target));   
        }

        if (MechanicsManager.Instance.Contains(target, MechanicType.BLEED) && MechanicsManager.Instance.Contains(target, MechanicType.IMPALE))
        {
            int bleedStack = MechanicsManager.Instance.GetMechanicsStack(target, MechanicType.BLEED);
            int impaleStack = MechanicsManager.Instance.GetMechanicsStack(target, MechanicType.IMPALE);
            target.TakeDamage(bleedStack * impaleStack, CombatManager.Instance.Player, true); 
            MechanicsManager.Instance.RemoveMechanic(target, MechanicType.IMPALE);
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}