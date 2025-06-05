using System;
using Game;
using UnityEngine;

public class EternalArmorPerk : BasePerk
{

    private EternalArmorPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (EternalArmorPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_START, EGamePhase.MECHANICS_FINISHED_TRY_REDUCE};
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
            case EGamePhase.PLAYER_TURN_START:
                GameActionHelper.SetMechanicGuard(m_perkData.MechanicType);
                break;
            case EGamePhase.MECHANICS_FINISHED_TRY_REDUCE:
                GameActionHelper.RemovePlayerMechanicGuard(m_perkData.MechanicType);
                break;
        }
    }
    
    
}
