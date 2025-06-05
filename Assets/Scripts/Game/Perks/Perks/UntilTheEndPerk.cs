using System;
using Game;
using UnityEngine;

public class UntilTheEndPerk : BasePerk
{

    private bool isConditionMet;
    
    private UntilTheEndPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (UntilTheEndPerkData)perkData;
    }

    public override void OnAdd()
    {
        OnPhaseActivate(EGamePhase.PLAYER_DAMAGED, null);
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
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_DAMAGED, EGamePhase.PLAYER_HEALED};
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
                if (playerHPPercentge < m_perkData.HPPercantegeTrigger)
                {
                    if (!isConditionMet)
                    {
                        isConditionMet = true;
                        GameActionHelper.AddMechanicToPlayer(m_perkData.Str, m_perkData.MechanicType, true,
                            m_perkData.Str);
                    }
                }

                break;

            case EGamePhase.PLAYER_HEALED:
                if (isConditionMet && playerHPPercentge > m_perkData.HPPercantegeTrigger)
                {
                    isConditionMet = false;
                    GameActionHelper.RemovePlayerMechanicGuard(m_perkData.MechanicType);
                }
                break;
        }
    }
}
