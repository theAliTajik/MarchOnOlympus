using System;
using Game;
using UnityEngine;

public class KairosPerk : BasePerk
{

    private KairosPerkData m_perkData;

    private bool isMechanicActive = false;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (KairosPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.MechanicAddedToFighter += OnMechanicAdded;
    }

    public override void OnRemove()
    {
        GameplayEvents.MechanicAddedToFighter -= OnMechanicAdded;
        if (isMechanicActive)
        {
            OnMechanicRemoved(MechanicType.FORTIFIED);
        }
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
        bool isPlayer = GameInfoHelper.CompareFighterToPlayer(fighter);
        if (isPlayer && mechanic.GetMechanicType() == m_perkData.MechanicTypeToCheck)
        {
            if (isMechanicActive)
            {
                return;
            }
            
            GameActionHelper.AddMechanicToPlayer(m_perkData.MechGain, m_perkData.MechanicToAdd);
            mechanic.OnEnd += OnMechanicRemoved;
            isMechanicActive = true;
        }
    }

    private void OnMechanicRemoved(MechanicType mechanicType)
    {
        Fighter player = GameInfoHelper.GetPlayer();
        GameActionHelper.ReduceMechanicStack(player, m_perkData.MechGain, m_perkData.MechanicToAdd);
        isMechanicActive = false;
    }
}
