using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlackBloodCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        BlackBloodCard c = (BlackBloodCard)cardData;
        MechanicsManager.Instance.AddMechanic(new StrenghtMechanic(c.Str, CombatManager.Instance.Player));
        MechanicsManager.Instance.AddMechanic(new BleedMechanic(c.Bleed, CombatManager.Instance.Player));

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}