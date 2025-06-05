using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleTroubleCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        DoubleTroubleCard c = (DoubleTroubleCard)cardData;

        int cardsPlayed = GameInfoHelper.CountNumOfCardsPlayed();
        int nextCardPlayed = cardsPlayed + 1;
        GameActionHelper.SetCardToBePlayedTwice(nextCardPlayed);

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if (phase == EGamePhase.CARD_SELECTED)
        {
            CardDisplay card = GameInfoHelper.CardsData.SelectedCard;
            card.CardInDeck.NormalState.SetDoesPerishOverride(true);
            card.CardInDeck.StanceState.SetDoesPerishOverride(true);
            card.RefreshUI();
            GameplayEvents.GamePhaseChanged -= OnPhaseChange;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }
}