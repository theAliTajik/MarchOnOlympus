using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmashCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        SmashCard c = (SmashCard)cardData;
        int buffsAmount = MechanicsManager.Instance.GetMechanicsCount(target, MechanicCategory.BUFF);

        if (buffsAmount > 0)
        {
            target.TakeDamage(c.DamageForEachBuff * buffsAmount, CombatManager.Instance.Player, true);    
        }
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(3, target));
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}