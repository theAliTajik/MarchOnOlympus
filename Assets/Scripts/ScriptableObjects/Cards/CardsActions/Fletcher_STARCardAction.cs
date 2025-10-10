using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fletcher_STARCardAction : BaseCardAction
{
    private Fletcher_STARCard m_data;
    private List<CardDisplay> m_arrowCardsFound;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Fletcher_STARCard)cardData;

        m_arrowCardsFound = GameInfoHelper.GetCardsWithName("Arrow", contains: true);
        foreach (var card in m_arrowCardsFound)
        {
            GameActionHelper.SetCardDescriptionOverride(card, ECardInDeckState.NORMAL, "+2 Damage");
        }
        GameActionHelper.AddExtraActionToCards(this, ExtraDamage);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void ExtraDamage(CardDisplay cardDisplay, Fighter fighter)
    {
        if (!m_arrowCardsFound.Contains(cardDisplay)) return;
        
        
        GameActionHelper.DamageFighter(fighter, GameInfoHelper.GetPlayer(), m_data.ExtraDamage);
    }
}