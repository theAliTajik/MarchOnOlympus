using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class WhetstoneCardAction : BaseCardAction
{
    private WhetstoneCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (WhetstoneCard)cardData;
        
        List<CardDisplay> cardDisplays = GameInfoHelper.GetAllCardsIn(CardStorage.HAND);

        if (cardDisplays.Count > 0)
        {
            GameActionHelper.AddExtraActionToCards(this, ExtraActionToAppendToCards);
        }

        for (var i = 0; i < cardDisplays.Count; i++)
        {
            string cardName = cardDisplays[i].CardInDeck.GetCardName();
            if (cardName == m_data.Name)
            {
                continue;
            }
            cardDisplays[i].CardInDeck.NormalState.SetTargetTypeOverride(TargetType.ENEMY);
            cardDisplays[i].CardInDeck.StanceState.SetTargetTypeOverride(TargetType.ENEMY);
            
            cardDisplays[i].CardInDeck.NormalState.SetDescriptionOverride(m_data.AddedDescriptionToCard, true);
        }
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void ExtraActionToAppendToCards(CardDisplay cardDisplay, Fighter target)
    {
        if (cardDisplay.CardInDeck.GetCardName() == m_data.Name)
        {
            return;
        }
        GameActionHelper.AddMechanicToFighter(target, m_data.ImpaleGain, MechanicType.IMPALE);
    }

}