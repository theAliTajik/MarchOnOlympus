using System;
using Game;
using UnityEngine;

public class EmergencyPerk : BasePerk
{

    private EmergencyPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (EmergencyPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

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
        if (m_perkData.ConditionHasBenMet)
        {
            return;
        }
        
        int playerHPPrecent = GameInfoHelper.GetPlayerHPPrecentage();
        if (playerHPPrecent < m_perkData.HPThresholdPercentage)
        {
            GameActionHelper.HealPlayer(m_perkData.Restore);
            m_perkData.ConditionHasBenMet = true;
        }
    }
}
