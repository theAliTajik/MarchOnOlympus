using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.ChangeTrackerService;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fletcher_PLUSCardAction : BaseCardAction
{
    private Fletcher_PLUSCard m_data;
    private List<BaseCardData> m_arrowCards;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Fletcher_PLUSCard)cardData;

        for (int i = 0; i < m_data.NumOfArrowsToSpawn; i++)
        {
            SpawnArrow();
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCardRewardSelected -= OnArrowCardSelected;
    }

    private void SpawnArrow()
    {
        if (m_arrowCards == null)
        {
            m_arrowCards = GetArrowCards();
        }
        
        GameplayEvents.OnCardRewardSelected += OnArrowCardSelected;
        GameActionHelper.GetSelectionFromPlayer(m_arrowCards);
    }

    private void OnArrowCardSelected(BaseCardData card)
    {
        GameplayEvents.OnCardRewardSelected -= OnArrowCardSelected;
        GameActionHelper.SpawnCard(card, CardStorage.DRAW_PILE);
    }

    public List<BaseCardData> GetArrowCards()
    {
        List<BaseCardData> ArrowCards = GameInfoHelper.GetCardsWithNameFromDB("Arrow", contains: true);

        if (ArrowCards == null || ArrowCards.Count == 0)
        {
            CustomDebug.LogWarning("Fletcher: Did not find any arrow cards in DB", Categories.Combat.Cards);
        }
        
        return ArrowCards;
    }

}