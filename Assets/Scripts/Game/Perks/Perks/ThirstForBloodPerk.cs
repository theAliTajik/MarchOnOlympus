using System;
using Game;
using Unity.VisualScripting;
using UnityEngine;

public class ThirstForBloodPerk : BasePerk
{

    private ThirstForBloodPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ThirstForBloodPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.ENEMY_KILLED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    } 

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        GameActionHelper.AddMechanicToPlayer(m_perkData.Str, MechanicType.STRENGTH);
    }
}
