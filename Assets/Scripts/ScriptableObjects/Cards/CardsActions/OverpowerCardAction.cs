using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverpowerCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        OverpowerCard c = (OverpowerCard)cardData;
        int finalDamage = target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);

        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            if (finalDamage > c.DamageThreshold)
            {
                Debug.Log("gave daze");
                MechanicsManager.Instance.AddMechanic(new DazeMechanic(c.Daze, target));
            }    
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}