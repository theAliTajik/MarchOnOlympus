using System;
using Game;
using UnityEngine;

public class PolumetisPerk : BasePerk
{

    private PolumetisPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (PolumetisPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.OnCardGainInvent += OnGainInvent;
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
    
    private void OnGainInvent(int i)
    {
        GameActionHelper.GainInvent(i);
    }

}
