using System;
using Game;
using UnityEngine;

public class MinimalistPerk : BasePerk
{

    private bool IsConditionMet;

    private MinimalistPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (MinimalistPerkData)perkData;
    }

    public override void OnAdd(){}

    public override void OnRemove()
    {
        if (IsConditionMet)
        {
            GameActionHelper.DecreaseDrawAmount(1);
        }
    }
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED, EGamePhase.PLAYER_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return 7;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int numOfCards = GameInfoHelper.CountNumOfCardsInDeck(CardStorage.ALL);
        switch (phase)
        {
            case EGamePhase.CARD_DRAW_FINISHED:
                if (numOfCards < m_perkData.CardsAmountThreshold)
                {
                    GameActionHelper.GainEnergy(m_perkData.EnergyGain);
                    GameActionHelper.AddMechanicToPlayer(m_perkData.BlockGain, MechanicType.BLOCK);
                }
                break;
            case EGamePhase.PLAYER_TURN_END:
                if (!IsConditionMet && numOfCards < m_perkData.CardsAmountThreshold)
                {
                    GameActionHelper.IncreaseDrawAmount(1);
                }

                IsConditionMet = true;
                break;
            
        }
        
        
    }
}
