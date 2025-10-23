using System;
using Game;
using UnityEngine;

public class PhronesisPerk : BasePerk
{

    private PhronesisPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (PhronesisPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.OnInventPlayed += OnInvent;
    }

    private void OnInvent(InventFinisher inventFinisher, int i)
    {
        GameActionHelper.GainInvent(m_perkData.Invent);
    }

    public override void OnRemove()
    {
        GameplayEvents.OnInventPlayed += OnInvent;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnInventPlayed += OnInvent;
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { };
        return phases;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
    }
}
