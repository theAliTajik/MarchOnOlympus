using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesperateMeasuresCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        DesperateMeasuresCard c = (DesperateMeasuresCard)cardData;

        CardDisplay randCard = GameInfoHelper.GetRandomCard(CardStorage.DISCARD_PILE);

        if (randCard is not null)
        {
            int cardCost = randCard.CardInDeck.CurrentState.GetEnergy();
            int restore = cardCost * c.CostMultiplierForRestore;
            GameActionHelper.PerishCard(randCard);

            if (CombatManager.Instance.CurrentStance == cardData.MStance)
            {
                restore = cardCost * c.StanceCostMultiplierForRestore;
            }

            GameActionHelper.HealPlayer(restore);
        }

        finishCallback?.Invoke();
        yield break;
    }

}