using System;
using Game;
using UnityEngine;

public class ThornfulPerk : BasePerk
{

    private ThornfulPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ThornfulPerkData)perkData;
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
        return 1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        GameActionHelper.DamageFighter(randEnemy, GameInfoHelper.GetPlayer(), m_perkData.Damage);
    }
}
