using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrenziedRegenerationCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        FrenziedRegenerationCard c = (FrenziedRegenerationCard)cardData;
        CombatManager.Instance.Player.Heal(c.Restore);
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            if (CombatManager.Instance.DamageDoneToEnemiesThisTurn > c.MinimumDamageToEnemies)
            {
                CombatManager.Instance.Player.Heal(c.RestoreIfMinimumMet);
            }
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}