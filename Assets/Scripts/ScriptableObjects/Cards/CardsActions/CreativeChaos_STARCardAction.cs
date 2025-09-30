using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreativeChaos_STARCardAction : BaseCardAction
{
    private CreativeChaos_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (CreativeChaos_STARCard)cardData;

        List<CardDisplay> cardsSpawned = new();
        for (int i = 0; i < m_data.NumOfCardsToSpawn; i++)
        {
            var card = GameActionHelper.SpawnRandomCard(CardStorage.HAND);
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.NORMAL, m_data.CardsCost);
            GameActionHelper.SetCardEnergyOverride(card, ECardInDeckState.STANCE, m_data.CardsCost);
            cardsSpawned.Add(card);
        }
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}