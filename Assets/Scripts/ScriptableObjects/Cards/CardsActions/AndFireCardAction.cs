using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndFireCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        AndFireCard c = (AndFireCard)cardData;
        MechanicsManager.Instance.AddMechanic(new StrenghtMechanic(c.Str, CombatManager.Instance.Player));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            GameActionHelper.TransformCard(cardDisplay, c.TransformCardName);    
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}