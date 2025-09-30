using System;
using Game;
using Game.ModifiableParam;
using UnityEngine;

public class GainOneEnergyEventPerk : BasePerk
{

    private GainOneEnergyEventPerkData m_perkData;
    private IParamModifier<int> m_addEnergy;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (GainOneEnergyEventPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_addEnergy = new AddValueModifier<int>(m_perkData.EnergyAmount);
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
                GameActionHelper.ModifyEnergy(m_addEnergy);
                RemoveSelf();
                break;
        }
    }
}
