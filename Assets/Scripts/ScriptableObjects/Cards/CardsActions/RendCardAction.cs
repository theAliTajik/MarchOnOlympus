using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class RendCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        RendCard c = (RendCard)cardData;

        MechanicsManager.Instance.AddMechanic(new BleedMechanic(c.Bleed, target));
        
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}