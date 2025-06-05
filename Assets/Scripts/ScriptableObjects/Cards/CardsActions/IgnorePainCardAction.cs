using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IgnorePainCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        IgnorePainCard c = (IgnorePainCard)cardData;
        MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(1, CombatManager.Instance.Player));
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new BlockMechanic(c.Block, CombatManager.Instance.Player));
        }
        
        

        finishCallback?.Invoke();
        yield break;
    }

}