using System;
using Game;
using UnityEngine;

public class NearDeathExperiancePerk : BasePerk
{

    private NearDeathExperiancePerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (NearDeathExperiancePerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        // EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};
        // return phases;

        return null;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        
    }
}
