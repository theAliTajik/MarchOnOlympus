using System;
using Game;
using UnityEngine;

public class MaximalistPerk : BasePerk
{

    private MaximalistPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (MaximalistPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_perkData.HasIncreasedHP = false;            
        OnPhaseActivate(EGamePhase.COMBAT_START, null);
    }

    public override void OnRemove()
    {
        if (m_perkData.HasIncreasedHP)
        {
            GameActionHelper.DecreasePlayerMaxHP(m_perkData.MaxHPIncrease);
            m_perkData.HasIncreasedHP = false;            
        }
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.COMBAT_START};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int numOfCardsInDeck = GameInfoHelper.CountNumOfCardsInDeck(CardStorage.ALL);
        if (numOfCardsInDeck > m_perkData.NumOfCards)
        {
            GameActionHelper.IncreasePlayerMaxHP(m_perkData.MaxHPIncrease);
            m_perkData.HasIncreasedHP = true;
        }
    }
}
