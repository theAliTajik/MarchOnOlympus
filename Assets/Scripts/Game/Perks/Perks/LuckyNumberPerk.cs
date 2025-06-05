using System;
using Game;
using UnityEngine;

public class LuckyNumberPerk : BasePerk
{

    private LuckyNumberPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (LuckyNumberPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_PLAYED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int CardsPlayed = GameInfoHelper.CountNumOfCardsPlayed();
        if (CardsPlayed == m_perkData.TriggerCardNumber)
        {
            GameActionHelper.GainEnergy(m_perkData.EnergyGain);
        }
        callback?.Invoke();
    }
}
