using System;
using Game;
using UnityEngine;

public class HemophilePerk : BasePerk
{

    private HemophilePerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (HemophilePerkData)perkData;
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
        return 8;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        GameActionHelper.AddMechanicToFighter(randEnemy,  m_perkData.Bleed, m_perkData.MechanicType);
    }
}
