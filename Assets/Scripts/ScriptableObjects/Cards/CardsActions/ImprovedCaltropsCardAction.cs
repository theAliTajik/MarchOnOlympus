using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedCaltropsCardAction : BaseCardAction
{
    private ImprovedCaltropsCard m_data;
    private Fighter m_target;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ImprovedCaltropsCard)cardData;
        m_target = target;
        GameActionHelper.AddExtraActionToCards(this, OnCardPlayed);
        GameplayEvents.OnInventPlayed += OnInventPlayed;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnInventPlayed -= OnInventPlayed;
    }

    private void OnInventPlayed()
    {
        ApplyBleed();
    }

    private void OnCardPlayed(CardDisplay card, Fighter arg2)
    {
        if(card.CardInDeck.GetCardType() != CardType.TECH) return;
        ApplyBleed();
    }

    private void ApplyBleed()
    {
        GameActionHelper.AddMechanicToFighter(m_target, m_data.Bleed, MechanicType.BLEED);
    }

}