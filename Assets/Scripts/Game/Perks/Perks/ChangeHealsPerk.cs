using System;
using Game;
using UnityEngine;

public class ChangeHealsPerk : BasePerk
{

    private ChangeHealsPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ChangeHealsPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_SPAWNED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        GameActionHelper.HealPlayer(m_perkData.Restore);
    }
}
