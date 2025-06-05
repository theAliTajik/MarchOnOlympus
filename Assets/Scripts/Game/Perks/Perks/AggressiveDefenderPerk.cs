using System;
using Game;
using UnityEngine;

public class AggressiveDefenderPerk : BasePerk
{

    private bool IsConditionMet;
    
    private AggressiveDefenderPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (AggressiveDefenderPerkData)perkData;
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
        return null;
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
        if (mechanic.GetMechanicType() == m_perkData.MechanicToTrack)
        {
            if (!GameInfoHelper.CompareFighterToPlayer(fighter))
            {
                return;
            }
            int StackOfMechanic = GameInfoHelper.GetMechanicStack(fighter, mechanic.GetMechanicType());
            if (StackOfMechanic >= m_perkData.TriggerAmount && !IsConditionMet)
            {
                GameActionHelper.SpawnCard(m_perkData.CardToSpawn, CardStorage.HAND);
                IsConditionMet = true;
            }
            else
            {
                IsConditionMet = false;
            }
        }
    }
}
