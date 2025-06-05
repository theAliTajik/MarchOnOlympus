using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AggressiveDefenderCardAction : BaseCardAction
{
    private AggressiveDefenderCard m_data;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f, cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target,
        CardDisplay cardDisplay)
    {
        m_data = (AggressiveDefenderCard)cardData;

        int numOfStance = GameInfoHelper.CountCardsByStance(m_data.StanceToCheck, CardStorage.ALL);
     
        if (numOfStance <= 0)
        {
            List<CardDisplay> stanceCards = GameInfoHelper.GetCardsBy(m_data.StanceToChange, CardStorage.ALL);
            for (var i = 0; i < stanceCards.Count; i++)
            {
                stanceCards[i].CardInDeck.NormalState.SetDescriptionOverride(m_data.DescriptionToAdd, true);
            }
            
            GameActionHelper.AddExtraActionToCards(this, ExtraAction);
        }

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            GameActionHelper.AddMechanicToPlayer(m_data.DextarityGain, MechanicType.DEXTERITY);
        }

        finishCallback?.Invoke();
        yield break;
    }

    private void ExtraAction(CardDisplay cardDisplay, Fighter fighter)
    {
        if (cardDisplay.CardInDeck.GetStance() == m_data.StanceToChange)
        {
            GameActionHelper.AddMechanicToPlayer(m_data.BlockGain, MechanicType.BLOCK);
        }
    }


}