using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComboStrikeCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        ComboStrikeCard c = (ComboStrikeCard)cardData;

        int damage = c.Damage * CombatManager.Instance.NumberOfCardsPlayedThisTurn;
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            damage = c.StanceDamage * CombatManager.Instance.NumberOfCardsPlayedThisTurn;
        }
        
        target.TakeDamage(damage, CombatManager.Instance.Player, true);

        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {

        finishCallback?.Invoke();
        yield break;
    }

}