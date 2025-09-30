using System;
using Game;
using UnityEngine;

public class NarcicistPerk : BasePerk
{

    private bool isConditionMet;

    private NarcicistPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (NarcicistPerkData)perkData;
    }

    public override void OnAdd()
    {
        OnPhaseActivate(EGamePhase.PLAYER_HEALED, null);
    }

    public override void OnRemove()
    {
        if (isConditionMet)
        {
            GameActionHelper.RemovePlayerMechanicGuard(m_perkData.MechanicType);
        }
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_HEALED, EGamePhase.PLAYER_DAMAGED};
        return phases;
    }

    public override float GetPriority()
    {
        return 6;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int playerHPPercentge = GameInfoHelper.GetPlayerHPPrecentage();
        switch (phase)
        {
            case EGamePhase.PLAYER_DAMAGED:
                if (isConditionMet && playerHPPercentge < m_perkData.HPPercentageTrigger)
                {
                    isConditionMet = false;
                    GameActionHelper.RemovePlayerMechanicGuard(m_perkData.MechanicType);
                }
                break;

            case EGamePhase.PLAYER_HEALED:
                if (!isConditionMet && playerHPPercentge > m_perkData.HPPercentageTrigger)
                {
                    isConditionMet = true;
                    GameActionHelper.AddMechanicToPlayer(m_perkData.Str, m_perkData.MechanicType,
                        m_perkData.Str);
                }
                break;
        }
    }
}
