using System;
using Game;
using UnityEngine;

public class TechnicalPerk : BasePerk
{

    private TechnicalPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (TechnicalPerkData)perkData;
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
        return 6;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        CardDisplay card = GameInfoHelper.GetLastCardPlayed();
        if (GameInfoHelper.IsCard(card, m_perkData.CardType))
        {
            GameActionHelper.AddMechanicToPlayer(m_perkData.Block, MechanicType.BLOCK);
        }
    }
}
