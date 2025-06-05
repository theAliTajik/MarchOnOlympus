using System;
using Game;
using UnityEngine;

public class PendantOfEnergyPerk : BasePerk
{

    private PendantOfEnergyPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (PendantOfEnergyPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_START};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int playerHpPercentage = GameInfoHelper.GetPlayerHPPrecentage();
        if (playerHpPercentage > m_perkData.AboveHPPrecent)
        {
            GameActionHelper.GainEnergy(m_perkData.EnergyGain);
        }
    }
}
