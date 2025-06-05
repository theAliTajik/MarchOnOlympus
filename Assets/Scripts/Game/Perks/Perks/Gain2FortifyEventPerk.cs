using System;
using Game;
using UnityEngine;

public class Gain2FortifyEventPerk : BasePerk
{

    private Gain2FortifyEventPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (Gain2FortifyEventPerkData)perkData;
    }

    public override void OnAdd()
    {
    }
    
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
                GameActionHelper.AddMechanicToPlayer(m_perkData.NumOfFortify, MechanicType.FORTIFIED);
                RemoveSelf();
                break;
        }
    }
}
