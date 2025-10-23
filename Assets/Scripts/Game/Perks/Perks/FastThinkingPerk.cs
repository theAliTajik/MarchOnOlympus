using System;
using Game;
using UnityEngine;

public class FastThinkingPerk : BasePerk
{

    private FastThinkingPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (FastThinkingPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.OnInventPlayed += OnInvent;
    }

    private void OnInvent(InventFinisher inventFinisher, int i)
    {
        GameActionHelper.AddMechanicToPlayer(m_perkData.Strength, MechanicType.STRENGTH);
    }

    public override void OnRemove()
    {
        GameplayEvents.OnInventPlayed -= OnInvent;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnInventPlayed -= OnInvent;
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
}
