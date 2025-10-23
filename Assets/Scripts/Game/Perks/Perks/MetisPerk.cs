using System;
using Game;
using UnityEngine;

public class MetisPerk : BasePerk
{

    private MetisPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (MetisPerkData)perkData;
    }

    public override void OnAdd(){}
    
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
