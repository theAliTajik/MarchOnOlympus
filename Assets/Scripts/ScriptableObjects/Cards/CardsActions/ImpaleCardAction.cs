using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImpaleCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        ImpaleCard c = (ImpaleCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new ImpaleMechanic(c.ImpaleAmount, target));
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}