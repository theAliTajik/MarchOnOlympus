using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenderCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {

        
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        int numOfBleedCards = GameInfoHelper.CountCardsByPack(CardPacks.BLEED, CardStorage.ALL);
        if (numOfBleedCards > 1) // this card is in bleed pack so there will always be at least one
        {
            MechanicsManager.Instance.AddMechanic(new BleedMechanic(numOfBleedCards-1, target));
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}