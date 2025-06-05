using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        PlanCard c = (PlanCard)cardData;
        MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(2, target));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new BlockMechanic(c.StanceBlockAmount, CombatManager.Instance.Player));
            CombatManager.Instance.ForceChangeStance(Stance.BERSERKER);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}