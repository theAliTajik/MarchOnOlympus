using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveForLaterCardAction : BaseCardAction
{
    private SaveForLaterCard m_data;

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (SaveForLaterCard)cardData;

        var cards = GetArrowCardsInDeck();
        int randCardIndex = UnityEngine.Random.Range(0, cards.Count);
        GameActionHelper.DiscardCard(cards[randCardIndex]);
        
        GameActionHelper.AddMechanicToPlayer(m_data.Strength, MechanicType.STRENGTH);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private List<CardDisplay> GetArrowCardsInDeck()
    {
        List<CardDisplay> cards = GameInfoHelper.GetCardsWithName("Arrow", contains: true);
        if (cards == null || cards.Count == 0)
        {
            CustomDebug.LogWarning("Save For Later: Did not find any arrow cards in deck", Categories.Combat.Cards);
        }
        
        return cards;
    }
    

}