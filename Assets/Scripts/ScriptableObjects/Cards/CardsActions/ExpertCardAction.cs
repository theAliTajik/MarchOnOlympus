using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpertCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ExpertCard c = (ExpertCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            GameActionHelper.TransformCard(cardDisplay, c.TranformCardName);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}