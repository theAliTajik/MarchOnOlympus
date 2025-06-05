using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirstForBloodCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        ThirstForBloodCard c = (ThirstForBloodCard)cardData;

        bool targetIsBleeding = MechanicsManager.Instance.Contains(target, MechanicType.BLEED);
        if (targetIsBleeding)
        {
            target.TakeDamage(c.Damage + c.ExtraDamageIfTargetBleeding, CombatManager.Instance.Player, true);
        }
        else
        {
            target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.Player.Heal(c.StanceRestoreHP);
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}