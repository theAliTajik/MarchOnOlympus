using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DelveInToChaosCardAction : BaseCardAction
{
    private DelveInToChaosCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (DelveInToChaosCard)cardData;
        List<CardDisplay> cardsToPerish = GetRandomCardsFromDeck(m_data.PerishCards);
        PerishCardsFromDeck(cardsToPerish);
        AddRandLegendaryCardsToDeck(m_data.NumOfLegendaryCards, CardStorage.DRAW_PILE);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private List<CardDisplay> GetRandomCardsFromDeck(int num)
    {
        List<CardDisplay> result = new List<CardDisplay>();
        for (int i = 0; i < num; i++)
        {
            result.Add(GameInfoHelper.GetRandomCard(CardStorage.ALL));
        }

        if (result.Count < num || result.Count <= 0)
        {
            CustomDebug.LogWarning("Delve into chaos: Did not Get Enough Random Cards", Categories.Combat.Cards);
        }
        
        return result;
    }
    
    private void PerishCardsFromDeck(List<CardDisplay> cards)
    {
        foreach (var card in cards)
        {
            GameActionHelper.PerishCard(card);
        }
    }

    private void AddRandLegendaryCardsToDeck(int num, CardStorage cardStorage)
    {
        List<BaseCardData> randCards = new List<BaseCardData>();

        for (int i = 0; i < num; i++)
        {
            randCards.Add(GameInfoHelper.GetRandomCard());
        }

        if (randCards.Count == 0)
        {
            CustomDebug.LogWarning("Delve into Chaos: did not get any random cards", Categories.Combat.Cards);
            return;
        }

        if (randCards.Count < num)
        {
            CustomDebug.LogWarning($"Delve into chaos: did not get enough rand cards. rand num: {randCards.Count}. target num: {num}", Categories.Combat.Cards);
        }

        for (int i = 0; i < randCards.Count; i++)
        {
            GameActionHelper.SpawnCard(randCards[i], cardStorage);
        }
        
    }

}