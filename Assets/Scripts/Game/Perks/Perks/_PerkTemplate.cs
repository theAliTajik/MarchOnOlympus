using System;
using Game;
using UnityEngine;

public class _PerkTemplate : BasePerk
{

    /*
    private _BasePerkData m_perkData;
    
    public void Config(BasePerkData perkData)
    {
        m_perkData = (_BasePerkData)perkData;
    }

*/
    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        // EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};
        // return phases;
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
