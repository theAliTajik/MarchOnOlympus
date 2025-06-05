using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptimizeCardAction : BaseCardAction
{
    private OptimizeCard m_data;
    private int m_multiplier;
    private Fighter m_target;

    private const CardStorage m_cardStorage = CardStorage.HAND;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (OptimizeCard)cardData;
        

        
        // set multiplier
        m_multiplier = m_data.CostMultiplierDamage;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            // set stance multiplier
            m_multiplier = m_data.StanceCostMultiplierDamage;
        }
        
        m_target = target;
  
        // choose a card
        CardPile cardPile = GameInfoHelper.GetCardPile(m_cardStorage);
        GameplayEvents.SendShowCardsForSelecting(cardPile);
        GameplayEvents.CardSelectedByPlayer += OnCardSelected;
        GameplayEvents.CardNotSelected += OnCardNotSelected;

        finishCallback?.Invoke();
        yield break;
    }
    
    public void OnCardSelected(CardDisplay cardDisplay)
    {
        DoAction(cardDisplay);
    }

    public void OnCardNotSelected()
    {
        CardDisplay randCard = GameInfoHelper.GetRandomCard(m_cardStorage); 
        DoAction(randCard);
    }

    public void DoAction(CardDisplay cardDisplay)
    {
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;
        GameplayEvents.CardNotSelected -= OnCardNotSelected;
        
        // get it's cost
        int cost = cardDisplay.CardInDeck.CurrentState.GetEnergy();
        // perish it
        GameActionHelper.PerishCard(cardDisplay);
        
        //damage
        int damage = cost * m_multiplier;
        m_target.TakeDamage(damage, CombatManager.Instance.Player, true);
        
    }

}