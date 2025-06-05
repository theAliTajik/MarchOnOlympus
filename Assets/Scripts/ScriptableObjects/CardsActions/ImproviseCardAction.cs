using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class ImproviseCardAction : BaseCardAction
{
    private List<CardDisplay> m_cardsSpawned = new List<CardDisplay>();
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ImproviseCard c = (ImproviseCard)cardData;
        
        int randCardAmount = c.CardSpawn;
        int cardCostOverride = c.CardCostOverride;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            randCardAmount = c.StanceCardSpawn;
            cardCostOverride = c.StanceCardCostOverride;
        }
        
        for (int i = 0; i < randCardAmount; i++)
        {
            int rand = UnityEngine.Random.Range(0, CardsDb.Instance.AllCards.Count);
            BaseCardData randCard = CardsDb.Instance.AllCards[rand].CardData;
            CardDisplay cardInstance = GameActionHelper.SpawnCard(randCard, CardStorage.HAND);
            GameActionHelper.SetCardEnergyOverride(cardInstance, ECardInDeckState.NORMAL, cardCostOverride);
            GameActionHelper.SetCardEnergyOverride(cardInstance, ECardInDeckState.STANCE, cardCostOverride);
            m_cardsSpawned.Add(cardInstance);
            
        }

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if (phase == EGamePhase.PLAYER_TURN_START)
        {
            for (var i = 0; i < m_cardsSpawned.Count; i++)
            {
                m_cardsSpawned[i].CardInDeck.NormalState.RemoveAllEnergyOverrides();
                m_cardsSpawned[i].CardInDeck.StanceState.RemoveAllEnergyOverrides();
            }
            GameplayEvents.GamePhaseChanged -= OnPhaseChange;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }
}