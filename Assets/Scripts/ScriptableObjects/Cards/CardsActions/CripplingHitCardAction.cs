using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class CripplingHitCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        CripplingHitCard c = (CripplingHitCard)cardData;
        
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(2, target));
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}