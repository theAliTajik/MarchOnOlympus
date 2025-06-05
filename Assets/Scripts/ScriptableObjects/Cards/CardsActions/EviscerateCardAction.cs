using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class EviscerateCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        EviscerateCard c = (EviscerateCard)cardData;
        
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        MechanicsManager.Instance.AddMechanic(new ImpaleMechanic(c.ImpaleAmount, target));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(1, CombatManager.Instance.Player));
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}