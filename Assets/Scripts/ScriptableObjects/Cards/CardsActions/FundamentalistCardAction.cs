using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FundamentalistCardAction : BaseCardAction
{
    private FundamentalistCard m_data;
    private int m_damage;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (FundamentalistCard)cardData;
        m_damage = m_data.BoostAmount;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            m_damage = m_data.StanceBoostAmount;
        }

        List<CardDisplay> allCards = GameInfoHelper.GetCardsBy(CardActionType.ATTACK, CardStorage.ALL);
        string description = string.Format(m_data.DescriptionToAddToCards, m_damage); 
        foreach (CardDisplay card in allCards)
        {
            GameActionHelper.SetCardDescriptionOverride(card, ECardInDeckState.NORMAL, description, true);
            GameActionHelper.SetCardTargetingTypeOverride(card, ECardInDeckState.NORMAL, TargetType.ENEMY);
            GameActionHelper.SetCardTargetingTypeOverride(card, ECardInDeckState.STANCE, TargetType.ENEMY);
        }
        
     
        finishCallback?.Invoke();
        
        GameActionHelper.AddExtraActionToCards(this, ActionToAddToCards);
        yield break;
    }

    private bool firstCard = true;
    private void ActionToAddToCards(CardDisplay cardDisplay, Fighter target)
    {
        if (firstCard)
        {
            firstCard = false;
            return;
        }
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_damage);
    }
}