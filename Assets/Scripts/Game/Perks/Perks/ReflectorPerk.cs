using System;
using Game;
using UnityEngine;

public class ReflectorPerk : BasePerk
{

    private ReflectorPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ReflectorPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.MechanicAddedToFighter += OnMechanicAdded;
    }

    private void OnMechanicAdded(Fighter fighter, BaseMechanic mech)
    {
        bool isPlayer = GameInfoHelper.CompareFighterToPlayer(fighter);
        if(!isPlayer) return;
        
        MechanicType mechType = mech.GetMechanicType();
        
        bool isDebuff = GameInfoHelper.CheckIfMechanicIsDebuff(mechType);
        if (!isDebuff) return;

        int stack = mech.Stack;
        GameActionHelper.RemoveMechanicFromPlayer(mechType);
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        GameActionHelper.AddMechanicToFighter(randEnemy, stack, mechType);
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
        return null;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
    }
}
