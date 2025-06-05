using System;
using Game;
using UnityEngine;

public class HornsOfMinotaurPerk : BasePerk
{

    private HornsOfMinotaurPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (HornsOfMinotaurPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

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
        GameActionHelper.AddMechanicToPlayer(m_perkData.Frenzy, MechanicType.FRENZY);
    }
}
