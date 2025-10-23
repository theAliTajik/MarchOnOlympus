using System;
using Game;
using UnityEngine;

public class ResilientPerk : BasePerk
{

    private ResilientPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ResilientPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.OnInventPlayed += OnInvent;
    }

    private void OnInvent(InventFinisher inventFinisher, int level)
    {
        if(level != m_perkData.InventLevelCondition) return;
        
        GameActionHelper.AddMechanicToPlayer(m_perkData.Block, MechanicType.BLOCK);
    }

    public override void OnRemove(){}
    
    private void OnDestroy(){}

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
