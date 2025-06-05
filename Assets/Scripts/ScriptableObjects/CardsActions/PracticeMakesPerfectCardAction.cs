using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PracticeMakesPerfectCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        PracticeMakesPerfectCard c = (PracticeMakesPerfectCard)cardData;

        int damage = c.Damage;
        damage += PracticeMakesPerfectCardActionExtensions.DamageIncrease;

        bool damageFatal = GameInfoHelper.CheckIfDamageToFighterIsFatal(target, damage + c.DamageIncreaseOnKillingBlow);
        if (damageFatal)
        {
            damage += c.DamageIncreaseOnKillingBlow;
        }
        
        target.TakeDamage(damage, CombatManager.Instance.Player, true);
        
        PracticeMakesPerfectCardActionExtensions.DamageIncrease += c.DamageIncrease;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}


public static class PracticeMakesPerfectCardActionExtensions
{
    public static int DamageIncrease;
}