using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class CriticalStrikeCardAction : BaseCardAction
{

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        CriticalStrikeCard c = (CriticalStrikeCard)cardData;
        int strikeCardCount = GameInfoHelper.CountCardsWithName("Strike", CardStorage.ALL);
                    
        target.TakeDamage(c.Damage * strikeCardCount, CombatManager.Instance.Player, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            target.TakeDamage(c.Damage * strikeCardCount, CombatManager.Instance.Player, true);
            CombatManager.Instance.ForceChangeStance(Stance.BATTLE);
        }


        finishCallback?.Invoke();
        yield break;

    }

}