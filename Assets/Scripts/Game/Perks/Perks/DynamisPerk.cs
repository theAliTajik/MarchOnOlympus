using System;
using Game;
using TMPro;
using UnityEngine;

public class DynamisPerk : BasePerk
{
    private bool isSelfTriggerd;
    private DynamisPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (DynamisPerkData)perkData;
    }

    public override void OnAdd()
    {
    }
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] {EGamePhase.MECHANIC_ADDED};
        return phases;
    }

    public override float GetPriority()
    {
        return 6;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        bool isPlayer = GameInfoHelper.CompareFighterToPlayer(GameInfoHelper.MechanicsData.MechanicsTarget);
        BaseMechanic mechanic = GameInfoHelper.MechanicsData.AddedMechanic;
        if (isPlayer && mechanic.GetMechanicType() == m_perkData.TriggerMechanicType)
        {
            if (isSelfTriggerd)
            {
                isSelfTriggerd = false;
                return;
            }
            else
            {
                isSelfTriggerd = true;
            }
            GameActionHelper.AddMechanicToPlayer(m_perkData.StrGain, MechanicType.STRENGTH);
        }
    }
}
