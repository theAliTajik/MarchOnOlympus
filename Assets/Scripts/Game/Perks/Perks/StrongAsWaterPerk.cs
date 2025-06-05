using System;
using Game;
using UnityEngine;

public class StrongAsWaterPerk : BasePerk
{

    private StrongAsWaterPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (StrongAsWaterPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.ExtraCardDrawn += OnCardDrawn;
    }

    public override void OnRemove()
    {
        GameplayEvents.ExtraCardDrawn -= OnCardDrawn;
    }

    private void OnDestroy()
    {
        GameplayEvents.ExtraCardDrawn -= OnCardDrawn;
    }

    public override EGamePhase[] GetPhases()
    {
        return null;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        throw new NotImplementedException();
    }

    private void OnCardDrawn()
    {
        int extraCards = GameInfoHelper.GetExtraDrawnCardsThisTurn();
        if (extraCards > 0 && extraCards % m_perkData.ExtraCardDrawAmountTrigger == 0)
        {
            GameActionHelper.AddMechanicToPlayer(m_perkData.StrGain, MechanicType.STRENGTH);
        }
    }
}
