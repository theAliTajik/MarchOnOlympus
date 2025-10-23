using System;
using Game;
using UnityEngine;

public class ProgressPerk : BasePerk
{

    private ProgressPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ProgressPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        UpgradeCards();
    }

    private void UpgradeCards()
    {
        // TODO missing upgrade system
    }
}
