using System;
using Game;
using UnityEngine;

public class TitanPerk : BasePerk
{

    private TitanPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (TitanPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

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
        CardDisplay lastCard = GameInfoHelper.GetLastCardPlayed();
        int cardEnergyUse = GameInfoHelper.GetCardsEnergy(lastCard);
        if (cardEnergyUse >= m_perkData.CardEnergyThreshold)
        {
            GameActionHelper.GainEnergy(m_perkData.EnergyGain);
        }
    }
}
