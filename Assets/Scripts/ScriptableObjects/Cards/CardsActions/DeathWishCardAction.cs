using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeathWishCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        DeathWishCard c = (DeathWishCard)cardData;
        
        int damage = c.Damage;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            damage = c.StanceDamage;
        }

        CombatManager.Instance.Player.TakeDamage(damage, CombatManager.Instance.Player, false);

        List<Fighter> enemies = EnemiesManager.Instance.GetAllEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].TakeDamage(damage, CombatManager.Instance.Player, true);
        }


        finishCallback?.Invoke();
        yield break;
    }

}