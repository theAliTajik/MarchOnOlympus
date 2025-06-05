using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class AllOutPerk : BasePerk
{

    private AllOutPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (AllOutPerkData)perkData;
    }

    public override void OnAdd()
    {
        OnPhaseActivate(EGamePhase.CARD_DRAW_FINISHED, null);
    }
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 6;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        List<CardDisplay> allCardsInHand = GameInfoHelper.GetAllCardsIn(CardStorage.HAND);
        bool allCardsMeetCondition = true;
        foreach (CardDisplay cardDisplay in allCardsInHand)
        {
            bool cardIsOfActionType = GameInfoHelper.IsCard(cardDisplay, m_perkData.actionType);
            if (!cardIsOfActionType)
            {
                allCardsMeetCondition = false;
                break;
            }
        }

        if (allCardsMeetCondition)
        {
            GameActionHelper.GainEnergy(m_perkData.EnergyGain);
        }
    }
}
