using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoRestForTheWickedCardAction : BaseCardAction
{
    private int restorePerCard = 0;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        NoRestForTheWickedCard c = (NoRestForTheWickedCard)cardData;

        int numOfStaceCardInDeck = GameInfoHelper.CountCardsByStance(c.CardStanceToCheck);

        if (numOfStaceCardInDeck <= 0)
        {
            restorePerCard = c.RestorePerCard;
            GameplayEvents.GamePhaseChanged += OnAttackRestore;
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    private void OnAttackRestore(EGamePhase phase)
    {
        if (phase == EGamePhase.ENEMY_DAMAGED)
        {
            CombatManager.Instance.Player.Heal(restorePerCard);
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnAttackRestore;
    }

}