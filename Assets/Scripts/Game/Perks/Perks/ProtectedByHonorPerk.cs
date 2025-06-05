using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ProtectedByHonorPerk : BasePerk
{

    private ProtectedByHonorPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ProtectedByHonorPerkData)perkData;
    }
    
    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.CARD_DRAW_FINISHED:
                GameActionHelper.AddMechanicToPlayer(m_perkData.BlockGain, MechanicType.BLOCK);
                callback?.Invoke();
                break;
        }
    }
}
