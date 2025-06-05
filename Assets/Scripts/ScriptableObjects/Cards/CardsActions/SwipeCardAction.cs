using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        SwipeCard c = (SwipeCard)cardData;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }

        int damage = c.Damage;
        int breserkerCardsCount = GameInfoHelper.CountCardsByStance(c.CardStanceToCount, CardStorage.HAND);
        int cardsInHandCount = GameInfoHelper.CountNumOfCardsInDeck(CardStorage.HAND);
        
        damage -= cardsInHandCount - breserkerCardsCount;
        target.TakeDamage(damage, CombatManager.Instance.Player, true);
        
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}