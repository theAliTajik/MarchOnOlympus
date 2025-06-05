using System;
using Game;
using UnityEngine;

public class HeavyHitterPerk : BasePerk
{

    private HeavyHitterPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (HeavyHitterPerkData)perkData;
    }

    public override void OnAdd()
    {
        
    }
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.ENEMY_DAMAGED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public void OnEnemyDamaged()
    {
        
    }
    
    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        
    }
}
