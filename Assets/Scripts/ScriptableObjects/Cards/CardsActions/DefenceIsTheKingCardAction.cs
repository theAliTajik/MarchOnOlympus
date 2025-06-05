using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceIsTheKingCardAction : BaseCardAction
{
    private DefenceIsTheKingCard m_data;
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (DefenceIsTheKingCard)cardData;

        List<CardDisplay> cards = GameInfoHelper.GetAllCardsOfPack(CardPacks.BLOCK, CardStorage.ALL);
        
        // override their description and add text
        foreach (CardDisplay cardDis in cards)
        {
            if (cardDis.CardInDeck.GetCardName() == m_data.Name)
            {
                continue;
            }

            cardDis.CardInDeck.NormalState.SetDescriptionOverride(m_data.DescriptionToAddToCards, true);
        }
        
        GameActionHelper.AddExtraActionToCards(this, ActionToAppendToCards);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void ActionToAppendToCards(CardDisplay card, Fighter target)
    {
        if (card.CardInDeck.GetCardPack() != CardPacks.BLOCK)
        {
            return;
        }

        if (card.CardInDeck.GetCardName() == m_data.Name)
        {
            return;
        }
        
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        GameActionHelper.DamageFighter(randEnemy, GameInfoHelper.GetPlayer(), m_data.Damage);
    }

}