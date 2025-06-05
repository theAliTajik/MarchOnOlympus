using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldsUpCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ShieldsUpCard c = (ShieldsUpCard)cardData;
        MechanicsManager.Instance.AddMechanic(new DazeMechanic(c.Daze, CombatManager.Instance.Player));
        MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(1, CombatManager.Instance.Player));

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(c.AdditionalFortifiy, CombatManager.Instance.Player));
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}