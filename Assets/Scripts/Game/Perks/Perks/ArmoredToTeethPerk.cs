using System;
using Game;
using UnityEngine;

public class ArmoredToTeethPerk : BasePerk
{

    private ArmoredToTeethPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ArmoredToTeethPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 6;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        GameActionHelper.AddMechanicToPlayer(m_perkData.Block, MechanicType.BLOCK);
    }
}
