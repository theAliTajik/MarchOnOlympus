using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PierceCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        PierceCard c = (PierceCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(2, target));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.ForceChangeStance(c.SwitchToStance);
            GameActionHelper.TransformCard(cardDisplay, c.TransformCardName);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}