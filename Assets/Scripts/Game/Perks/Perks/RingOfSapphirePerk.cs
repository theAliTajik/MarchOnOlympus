using System;
using Game;
using UnityEngine;

public class RingOfSapphirePerk : BasePerk
{

    private RingOfSapphirePerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (RingOfSapphirePerkData)perkData;
    }

    public override void OnAdd()
    {
        GameActionHelper.AddMechanicToPlayer(m_perkData.DexteratyGain, MechanicType.DEXTERITY);
    }
    
    public override void OnRemove(){}

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
