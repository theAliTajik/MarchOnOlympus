using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExposedThrustCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        ExposedThrustCard c = (ExposedThrustCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        
        MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(2, CombatManager.Instance.Player));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new ImpaleMechanic(c.ImpaleAmount, target));
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}