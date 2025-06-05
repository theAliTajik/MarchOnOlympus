using System;
using Game;
using UnityEngine;

public class TillTheEndPerk : BasePerk
{

    bool isConditionMet = false;
    private TillTheEndPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (TillTheEndPerkData)perkData;
    }

    public override void OnAdd()
    {
        OnPhaseActivate(EGamePhase.PLAYER_DAMAGED, null);
    }

    public override void OnRemove()
    {
        if (isConditionMet)
        {
            GameActionHelper.RemovePlayerMechanicGuard(MechanicType.STRENGTH);
            GameActionHelper.ReduceMechanicStack(GameInfoHelper.GetPlayer(), m_perkData.StrGain, MechanicType.STRENGTH);
        }
    }
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_DAMAGED, EGamePhase.PLAYER_HEALED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int playerHPPercentage = GameInfoHelper.GetPlayerHPPrecentage();

        switch (phase)
        {
            case EGamePhase.PLAYER_DAMAGED:
                if (!isConditionMet && playerHPPercentage < m_perkData.HPThresholdPercentage)
                {
                    GameActionHelper.AddMechanicToPlayer(m_perkData.StrGain, MechanicType.STRENGTH, true, m_perkData.StrGain);
                    isConditionMet = true;
                }
                break;
            case EGamePhase.PLAYER_HEALED:
                if(isConditionMet)
                {
                    isConditionMet = false;
                    GameActionHelper.RemovePlayerMechanicGuard(MechanicType.STRENGTH);
                    GameActionHelper.ReduceMechanicStack(GameInfoHelper.GetPlayer(), m_perkData.StrGain, MechanicType.STRENGTH);
                }
                break;
        }
        
        
    }
}
