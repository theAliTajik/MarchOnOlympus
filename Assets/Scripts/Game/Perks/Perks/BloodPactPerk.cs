using System;
using Game;
using UnityEngine;

public class BloodPactPerk : BasePerk
{

    private BloodPactPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (BloodPactPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameActionHelper.GainEnergy(m_perkData.EnergyGain);
        Fighter fighter = GameInfoHelper.GetPlayer();
        GameActionHelper.DamageFighter(fighter, GameInfoHelper.GetPlayer(), m_perkData.LoseHP);
    }
    
    public override void OnRemove(){}

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
