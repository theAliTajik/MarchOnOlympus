using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class FletchingPerk : BasePerk
{

    private FletchingPerkData m_perkData;
    private CardDisplay m_modifiedCard;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (FletchingPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.OnInventPlayed += OnInvent;
    }

    private void OnInvent(InventFinisher inventFinisher, int level)
    {
        if (level != m_perkData.InventLevelCondition) return;

        List<CardDisplay> card = GameActionHelper.DrawCards(1);
        if (card == null || card.Count < 1)
        {
            CustomDebug.LogWarning("Could not draw card or drawn card was not returned", Categories.Perks.Root);
            return;
        }

        var modifier = new SetValueModifier<int>(m_perkData.CardCostOverride);
        GameActionHelper.ModifyCardEnergy(card[0], modifier);

        m_modifiedCard = card[0];
    }

    public override void OnRemove()
    {
        GameplayEvents.OnInventPlayed -= OnInvent;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnInventPlayed -= OnInvent;
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
    }
}
