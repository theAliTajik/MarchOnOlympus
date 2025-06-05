using System;
using Game;
using UnityEngine;

public class LastCallPerk : BasePerk
{

    private LastCallPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (LastCallPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_perkData.CurrentDrawAmount = 0;
        m_perkData.CurrentDrawExtraGivenToPlayer = 0;
    }

    public override void OnRemove()
    {
        if (m_perkData.CurrentDrawExtraGivenToPlayer > 0)
        {
            GameActionHelper.DecreaseDrawAmount(m_perkData.CurrentDrawExtraGivenToPlayer);
            m_perkData.CurrentDrawExtraGivenToPlayer = 0;
        }
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_DAMAGED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        //when the player gets damaged calculate how many extra cards need to be drawn;
        if (m_perkData == null || m_perkData.PerMissingPercentage <= 0)
        {
            return;
        }
        
        int PlayerHPPercentage = GameInfoHelper.GetPlayerHPPrecentage();
        
        m_perkData.CurrentDrawAmount = 0;
        for (int i = 1; i <= 100 / m_perkData.PerMissingPercentage; i++)
        {
            if (PlayerHPPercentage <= 100 - (m_perkData.PerMissingPercentage * i))
            {
                m_perkData.CurrentDrawAmount += m_perkData.DrawExtra;
            }
            else
            {
                break;
            }
        }

        if (m_perkData.CurrentDrawExtraGivenToPlayer > 0)
        {
            GameActionHelper.DecreaseDrawAmount(m_perkData.CurrentDrawExtraGivenToPlayer);
            m_perkData.CurrentDrawExtraGivenToPlayer = 0;
        }

        if (m_perkData.CurrentDrawAmount > 0)
        {
            GameActionHelper.IncreaseDrawAmount(m_perkData.CurrentDrawAmount);
            m_perkData.CurrentDrawExtraGivenToPlayer = m_perkData.CurrentDrawAmount;
        }
    }
}
