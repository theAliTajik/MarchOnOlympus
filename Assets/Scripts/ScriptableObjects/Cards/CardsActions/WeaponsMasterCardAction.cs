using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponsMasterCardAction : BaseCardAction
{
    public override void CardDrawn(BaseCardData cardData)
    {
        WeaponsMasterCard c = (WeaponsMasterCard)cardData;
        CombatManager.Instance.Player.TakeDamage(c.DamageWhenDrawn, CombatManager.Instance.Player, false);
    }

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        WeaponsMasterCard c = (WeaponsMasterCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}