using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllInCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        AllInCard c = (AllInCard)cardData;
        int damage = c.Damage;

        int defStanceAmount = GameInfoHelper.CountCardsByStance(Stance.DEFENCIVE, CardStorage.HAND);
        if (defStanceAmount <= 0)
        {
            damage = c.DamageIfStance;
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            damage = c.StanceDamage;
            if (defStanceAmount <= 0)
            {
                damage = c.StanceExtraDamageIfStance;
            }
        }
        
        
        target.TakeDamage(damage, CombatManager.Instance.Player, true);

        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}