using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OneFitsAllCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {
        List<CardDisplay> allCardsInHand = GameInfoHelper.GetAllCardsIn(CardStorage.ALL);

        StrikeCard strikeCardData = Resources.Load<StrikeCard>("CardsData/Strike");
        if (strikeCardData == null)
        {
            Debug.LogWarning("Strike Card Data is null");
            finishCallback?.Invoke();
            yield break;
        }
        
        if (allCardsInHand.Count != 0)
        {
            foreach (CardDisplay card in allCardsInHand)
            {
                GameActionHelper.ChangeCard(card, strikeCardData);
            }
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}