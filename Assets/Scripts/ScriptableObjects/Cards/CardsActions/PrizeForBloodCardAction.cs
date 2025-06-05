using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrizeForBloodCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        PrizeForBloodCard c = (PrizeForBloodCard)cardData;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        }
        
        int damageThisTurn = CombatManager.Instance.DamageDoneToEnemiesThisTurn;
        if (damageThisTurn > 0)
        {
            CombatManager.Instance.Player.Heal(damageThisTurn);
        }
        
        
        
        
        

        finishCallback?.Invoke();
        yield break;
    }

}