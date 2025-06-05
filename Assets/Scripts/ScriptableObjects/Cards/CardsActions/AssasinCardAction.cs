using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AssasinCardAction : BaseCardAction
{


    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    public override void NotUsed(BaseCardData cardData)
    {
        AssasinCard c = (AssasinCard)cardData;

        CombatManager.Instance.Player.TakeDamage(c.Damage, CombatManager.Instance.Player, false);
    }
}