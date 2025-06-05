using System;
using Game;
using UnityEngine;

public class SerendipityPerk : BasePerk
{

    private SerendipityPerkData m_perkData;

    private CardDisplay m_randCard;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (SerendipityPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_randCard = GameInfoHelper.GetRandomCard(CardStorage.ALL);
        
        GameActionHelper.SetCardEnergyOverride(m_randCard, ECardInDeckState.NORMAL, 0);
        GameActionHelper.SetCardEnergyOverride(m_randCard, ECardInDeckState.STANCE, 0);
    }

    public override void OnRemove()
    {
        GameActionHelper.RemoveCardEnergyOverride(m_randCard, ECardInDeckState.NORMAL);
        GameActionHelper.RemoveCardEnergyOverride(m_randCard, ECardInDeckState.STANCE);
    }
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        return null;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        throw new NotImplementedException();
    }
}
