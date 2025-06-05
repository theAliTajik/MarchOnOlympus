using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreparationCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        PreparationCard c = (PreparationCard)cardData;
        CombatManager.Instance.DiscardCard(c.DiscardCard);
        CombatManager.Instance.DrawCard(c.DrawCard);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.DrawCard(c.StanceDrawCard);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}