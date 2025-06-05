using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndParryCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(1, CombatManager.Instance.Player));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.ForceChangeStance(Stance.DEFENCIVE);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}