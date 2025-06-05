using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FortuneFavorsTheBoldCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        FortuneFavorsTheBoldCard c = (FortuneFavorsTheBoldCard)cardData;
        
        
        //get top card of draw pile
        CardDisplay cd = GameInfoHelper.GetTopCardIn(CardStorage.DRAW_PILE);
        // get random enemy 
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        
        // play top card to random enemy
        GameActionHelper.PlayCard(cd, randEnemy);
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}