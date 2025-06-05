using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObliterateCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        ObliterateCard c = (ObliterateCard)cardData;
        
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            yield return new WaitForSeconds(c.DelayBetweenAttacks);
            target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}