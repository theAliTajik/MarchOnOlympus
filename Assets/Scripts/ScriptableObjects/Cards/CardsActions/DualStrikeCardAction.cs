using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualStrikeCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        DualStrikeCard c = (DualStrikeCard)cardData;
       
        StartCoroutine(WaitAndExecute(finishCallback, c, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, DualStrikeCard c, Fighter target)
    {
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        yield return new WaitForSeconds(c.DelayBetweenAttacks);
        if (target != null)
        {
            target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        }

        yield return new WaitForSeconds(c.DelayBetweenAttacks);
        finishCallback?.Invoke();
    }
}
