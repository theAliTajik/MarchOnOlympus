using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class TheOnlyOptionCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        TheOnlyOptionCard c = (TheOnlyOptionCard)cardData;

        int numOfBreserkerCards = GameInfoHelper.CountCardsByStance(Stance.BERSERKER, CardStorage.ALL);
        if (numOfBreserkerCards > 0)
        {
            target.TakeDamage(numOfBreserkerCards, CombatManager.Instance.Player, true);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}