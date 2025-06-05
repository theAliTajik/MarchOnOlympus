using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalStrikeCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        TacticalStrikeCard c = (TacticalStrikeCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.SpawnCard("Tactical Strike", CardStorage.DISCARD_PILE);
        }

        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {

        finishCallback?.Invoke();
        yield break;
    }

}