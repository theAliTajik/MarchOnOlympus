using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class RevelInBloodCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        RevelInBloodCard c = (RevelInBloodCard)cardData;
        Fighter.DamageContext context = target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);

        MechanicsManager.Instance.AddMechanic(new BleedMechanic(context.ModifiedDamage, target));

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            int targetsBleed = MechanicsManager.Instance.GetMechanicsStack(target, MechanicType.BLEED);
            MechanicsManager.Instance.AddMechanic(new BlockMechanic(targetsBleed, CombatManager.Instance.Player));
        }


        finishCallback?.Invoke();
        yield break;
    }

}