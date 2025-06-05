using System;
using Game;
using UnityEngine;

public class CarryOverPerk : BasePerk
{

    private CarryOverPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (CarryOverPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END, EGamePhase.PLAYER_TURN_START};
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
                GameActionHelper.GainEnergy(m_perkData.AmountToCarryOver);
                break;
            case EGamePhase.PLAYER_TURN_END:
                m_perkData.AmountToCarryOver = GameInfoHelper.GetCurrentEnergy();
                break;
        }
    }
}
