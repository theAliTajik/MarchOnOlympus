using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AmokCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        AmokCard c = (AmokCard)cardData;


        bool damagePlayer = false;
        List<int> damages = new List<int>();
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            damages.AddRange(c.StanceDamages);
            damagePlayer = true;
        }
        else
        {
            damages.AddRange(c.Damages);
        }
        
        Fighter randEnemy = EnemiesManager.Instance.GetRandomEnemy();
        randEnemy.TakeDamage(damages[0], CombatManager.Instance.Player, true);
        
        yield return new WaitForSeconds(1f);
        
        for (int i = 1; i < damages.Count; i++)
        {
            bool finishedAnim = false;
            float waitTime = CombatManager.Instance.Player.PlayAttackAnimation(() => finishedAnim = true);
            yield return new WaitForSeconds(waitTime);
            randEnemy = EnemiesManager.Instance.GetRandomEnemy();
            randEnemy.TakeDamage(damages[i], CombatManager.Instance.Player, true);
            yield return new WaitUntil(() => finishedAnim);
        }

        if (damagePlayer)
        {
            GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), GameInfoHelper.GetPlayer(), c.DamageToSelf, false);
        }
        finishCallback?.Invoke();
        yield break;
    }

}