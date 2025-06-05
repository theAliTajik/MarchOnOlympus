using System;
using Game;
using UnityEngine;

public class LessIsMorePerk : BasePerk
{

    private LessIsMorePerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (LessIsMorePerkData)perkData;
    }

    public override void OnAdd()
    {
        m_perkData.ConditionMet = false;
    }
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END, EGamePhase.PLAYER_TURN_START };
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
                if (m_perkData.ConditionMet)
                {
                    GameActionHelper.GainEnergy(m_perkData.EnergyGain);
                    m_perkData.ConditionMet = false;    
                }
                break;
            case EGamePhase.PLAYER_TURN_END:
                int cardsPlayedThisTurn = GameInfoHelper.CountNumOfCardsPlayedThisTurn();
                Debug.Log("cards played this turn: " + cardsPlayedThisTurn);
                if (cardsPlayedThisTurn <= m_perkData.CardThreshold)
                {
                    m_perkData.ConditionMet = true;
                }

                break;
        }
    }
}
