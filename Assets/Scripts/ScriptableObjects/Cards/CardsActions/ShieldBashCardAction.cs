using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldBashCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        ShieldBashCard c = (ShieldBashCard)cardData;
        MechanicsManager.Instance.AddMechanic(new DazeMechanic(c.Daze, target));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            target.TakeDamage(c.StanceDamage, CombatManager.Instance.Player, true);
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}