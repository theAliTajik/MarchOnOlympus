using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class TechyPerk : BasePerk
{

    private TechyPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (TechyPerkData)perkData;
    }

    public override void OnAdd()
    {
        OnPhaseActivate(EGamePhase.COMBAT_START, null);
    }
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.COMBAT_START};
        return phases;
    }

    public override float GetPriority()
    {
        return 2;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        List<CardDisplay> AllCards = GameInfoHelper.GetAllCardsIn(CardStorage.ALL);
        List<CardDisplay> TechCards = new List<CardDisplay>();
        foreach (CardDisplay cardDisplay in AllCards)
        {
            if (GameInfoHelper.IsCard(cardDisplay, CardType.TECH))
            {
                TechCards.Add(cardDisplay);
            }
        }
        
        CardDisplay randTechCard = TechCards[UnityEngine.Random.Range(0, TechCards.Count)];
        
        GameActionHelper.SetCardEnergyOverride(randTechCard, ECardInDeckState.NORMAL, 0);
        GameActionHelper.SetCardEnergyOverride(randTechCard, ECardInDeckState.STANCE, 0);
    }
}
