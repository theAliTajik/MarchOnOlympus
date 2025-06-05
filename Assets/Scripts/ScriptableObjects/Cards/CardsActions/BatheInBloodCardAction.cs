using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BatheInBloodCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        
        if (MechanicsManager.Instance.Contains(target, MechanicType.BLEED))
        {
            int amount = MechanicsManager.Instance.GetMechanicsStack(target, MechanicType.BLEED);
            MechanicsManager.Instance.AddMechanic(new BleedMechanic(amount, target));
        }
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new FrenzyMechanic(2, CombatManager.Instance.Player));    
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}