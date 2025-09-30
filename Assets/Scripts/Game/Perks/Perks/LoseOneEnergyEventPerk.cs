using System;
using Game;
using Game.ModifiableParam;
using UnityEngine;

public class LoseOneEnergyEventPerk : BasePerk
{

    private LoseOneEnergyEventPerkData m_perkData;
    private IParamModifier<int> m_removeEnergy;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (LoseOneEnergyEventPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_removeEnergy = new AddValueModifier<int>(-m_perkData.EnergyAmount);
    }
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_START};
        return phases;
    }

    public override float GetPriority()
    {
        return 1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.PLAYER_TURN_START:
                GameActionHelper.ModifyEnergy(m_removeEnergy);
                RemoveSelf();
                break;
        }
    }
}
