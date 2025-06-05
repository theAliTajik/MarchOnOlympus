using System;
using Game;
using UnityEngine;

public class GainBleedEachCombatEventPerk : BasePerk
{

    private GainBleedEachCombatEventPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (GainBleedEachCombatEventPerkData)perkData;
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
        switch (phase)
        {
            case EGamePhase.CARD_DRAW_FINISHED:
                GameActionHelper.AddMechanicToPlayer(m_perkData.Bleed, MechanicType.BLEED);
                break;
                
        }
    }
}
