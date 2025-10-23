using System;
using Game;
using UnityEngine;

public class TriggerHappyPerk : BasePerk
{

    private TriggerHappyPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (TriggerHappyPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.OnInventPlayed += OnInventPlayed;
    }

    public override void OnRemove()
    {
        GameplayEvents.OnInventPlayed -= OnInventPlayed;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnInventPlayed -= OnInventPlayed;
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_PLAYED};
        return phases;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        var card = GameInfoHelper.GetLastCardPlayed();
        bool isBomb = IsCardBomb(card);

        if (isBomb)
        {
            Restore();
        }
    }

    private bool IsCardBomb(CardDisplay cardDisplay)
    {
        //TODO: fix this
        return false;
    }
    
    private void OnInventPlayed(InventFinisher inventFinisher, int i)
    {
        //TODO: make this parameter finisher instead of int 
    }

    private void Restore()
    {
        GameActionHelper.HealPlayer(m_perkData.Restore);
    }

}
