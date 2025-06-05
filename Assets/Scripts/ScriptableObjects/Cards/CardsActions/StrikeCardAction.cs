using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrikeCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StrikeCard c = (StrikeCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new BleedMechanic(c.Bleed, target));
        }
        
        finishCallback?.Invoke();
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {
        yield return new WaitForSeconds(delay);
        
    }

}
