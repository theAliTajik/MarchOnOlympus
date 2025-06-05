using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaltropsCardAction : BaseCardAction
{
    private CaltropsCard m_data;
    private Fighter m_Target;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (CaltropsCard)cardData;
        m_Target = target;
        
        GameplayEvents.GamePhaseChanged += OnPhaseChanged;
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
    }

    private void OnPhaseChanged(EGamePhase phase)
    {
        if (phase == EGamePhase.CARD_PLAYED)
        {
            CardDisplay lastCard = GameInfoHelper.GetLastCardPlayed();
            if (lastCard.CardInDeck.GetCardName() == m_data.Name)
            {
                return;
            }
            if (lastCard.CardInDeck.GetCardType() == m_data.CardType)
            {
                MechanicsManager.Instance.AddMechanic(new BleedMechanic(m_data.Bleed, m_Target));
            }
        }
    }

}