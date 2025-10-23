using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class MasterArcherPerk : BasePerk
{

    private MasterArcherPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (MasterArcherPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        var arrowCards = GameInfoHelper.GetCardsWithName("Arrow", contains: true);

        int numOfAllCards = GameInfoHelper.CountNumOfCardsInDeck(CardStorage.ALL);

        int numOfNonArrowCards = numOfAllCards - arrowCards.Count;
        if (numOfNonArrowCards < m_perkData.NonArrowCardsThreshold)
        {
            UpgradeArrowCards(arrowCards);
        }
    }

    private void UpgradeArrowCards(List<CardDisplay> cards)
    {
        GameActionHelper.AddExtraActionToCards(this, CardUpgradedAction);
        foreach (var card in cards)
        {
            GameActionHelper.SetCardDescriptionOverride(card, ECardInDeckState.NORMAL, m_perkData.CardUpgradedActionDescription, true);
        }
    }

    private static void CardUpgradedAction(CardDisplay cardDisplay, Fighter fighter)
    {
        bool isArrow = GameInfoHelper.DoesCardNameContain("Arrow", cardDisplay);
        if (!isArrow) return;
        
        GameActionHelper.DamageFighter(fighter, GameInfoHelper.GetPlayer(), damage:2, doesReturnToSender:false);
        GameActionHelper.HealPlayer(amount:1);
    }
}
