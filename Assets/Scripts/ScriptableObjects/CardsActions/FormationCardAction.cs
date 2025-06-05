using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationCardAction : BaseCardAction
{
    private FormationCard m_data;
    List<CardDisplay> m_cardThatAttack = new List<CardDisplay>();
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target,
        CardDisplay cardDisplay)
    {
        m_data = (FormationCard)cardData;

        if (m_cardThatAttack.Count > 0)
        {
            for (var i = 0; i < m_cardThatAttack.Count; i++)
            {
                GameActionHelper.RemoveCardDescriptionOverride(m_cardThatAttack[i], ECardInDeckState.NORMAL);
                GameActionHelper.RemoveCardTargetingTypeOverride(m_cardThatAttack[i], ECardInDeckState.NORMAL);
                GameActionHelper.RemoveCardTargetingTypeOverride(m_cardThatAttack[i], ECardInDeckState.STANCE);
            }
        }

        // set num of random cards
        int numOfRandCards = m_data.NumOfRandCards;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            numOfRandCards = m_data.StanceNumOfRandCards;
        }
        
        // pick x random attack cards
        m_cardThatAttack = GameInfoHelper.GetCardsBy(CardActionType.ATTACK, CardStorage.ALL);

        if (m_cardThatAttack.Count > numOfRandCards)
        {
            int diff = m_cardThatAttack.Count - numOfRandCards;
            m_cardThatAttack.RemoveRange(0, diff);
        }
        
        // append damage buff
        GameActionHelper.AddExtraActionToCards(this, ActionToAppend);
        
        for (var i = 0; i < m_cardThatAttack.Count; i++)
        {
            GameActionHelper.SetCardDescriptionOverride(m_cardThatAttack[i], ECardInDeckState.NORMAL, m_data.DescriptionToAppendToCards, true);
            GameActionHelper.SetCardTargetingTypeOverride(m_cardThatAttack[i], ECardInDeckState.NORMAL, TargetType.ENEMY);
            GameActionHelper.SetCardTargetingTypeOverride(m_cardThatAttack[i], ECardInDeckState.STANCE, TargetType.ENEMY);
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    public void ActionToAppend(CardDisplay card, Fighter target)
    {
        if (m_cardThatAttack.Contains(card))
        {
            GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.DamageBuff);
        }
    }

}