using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExecuteCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ExecuteCard c = (ExecuteCard)cardData;
        int damage = c.Damage;
        if (GameInfoHelper.CheckIfPlayerHPIsUnderACertainPrecent(c.HPPrecent))
        {
            damage += c.AdditionalDamage;
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            damage = c.StanceDamage;
            if (GameInfoHelper.CheckIfPlayerHPIsUnderACertainPrecent(c.StanceHPPrecent))
            {
                damage += c.StanceAdditionalDamage;
            }   
        }
        
        target.TakeDamage(damage, CombatManager.Instance.Player, true);

        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}