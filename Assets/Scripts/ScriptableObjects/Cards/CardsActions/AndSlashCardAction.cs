using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndSlashCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        AndSlashCard c = (AndSlashCard)cardData;
        
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        MechanicsManager.Instance.AddMechanic(new BleedMechanic(c.BleedAmount, target));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.ForceChangeStance(Stance.BERSERKER);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}