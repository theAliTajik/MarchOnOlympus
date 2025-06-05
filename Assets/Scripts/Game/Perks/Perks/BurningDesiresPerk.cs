using System;
using Game;
using UnityEngine;

public class BurningDesiresPerk : BasePerk
{

    private BurningDesiresPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (BurningDesiresPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.MechanicAddedToFighter += OnMechanicAdded;

    }

    public override void OnRemove()
    {
        GameplayEvents.MechanicAddedToFighter -= OnMechanicAdded;

    }

    private void OnDestroy()
    {
        GameplayEvents.MechanicAddedToFighter -= OnMechanicAdded;

    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        throw new NotImplementedException();
    }
    
    
    private void OnMechanicAdded(Fighter fighter, BaseMechanic mechanic)
    {
        bool isPlayer = GameInfoHelper.CompareFighterToPlayer(fighter);
        if (!isPlayer && mechanic.GetMechanicType() == m_perkData.MechanicTypeToCheck)
        {
            GameActionHelper.AddMechanicToPlayer(m_perkData.ImproviseGain, MechanicType.IMPROVISE);
        }
    }
}
