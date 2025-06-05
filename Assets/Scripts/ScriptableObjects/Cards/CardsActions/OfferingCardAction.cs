using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class OfferingCardAction : BaseCardAction
{
    private OfferingCard m_data;
    private Fighter m_target;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (OfferingCard)cardData;
        
        CardPile cardPile = GameInfoHelper.GetCardPile(CardStorage.HAND);
        GameplayEvents.SendShowCardsForSelecting(cardPile);
        GameplayEvents.CardSelectedByPlayer += OnCardSelected;
        GameplayEvents.CardNotSelected += OnCardNotSelected;
        
        m_target = target;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }


    public void OnCardSelected(CardDisplay cardDisplay)
    {
        DoAction(cardDisplay);
    }

    public void OnCardNotSelected()
    {
        CardDisplay randCard = GameInfoHelper.GetRandomCard(CardStorage.HAND); 
        DoAction(randCard);
    }

    public void DoAction(CardDisplay cardDisplay)
    {
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;
        GameplayEvents.CardNotSelected -= OnCardNotSelected;
        int cardCost = GameInfoHelper.GetCardsEnergy(cardDisplay);
        
        GameActionHelper.PerishCard(cardDisplay);
        
        if (cardCost > m_data.CostThreshold)
        {
            GameActionHelper.SpawnCard(m_data.CardToSpawn, CardStorage.HAND);
        }
        
        
        m_target.TakeDamage(m_data.CostDamageMultiplier * cardCost, CombatManager.Instance.Player, true);
        
    }

    private void OnDestroy()
    {
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;
        GameplayEvents.CardNotSelected -= OnCardNotSelected;
    }
}