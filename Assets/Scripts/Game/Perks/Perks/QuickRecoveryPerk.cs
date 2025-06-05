using System;
using Game;
using UnityEngine;

public class QuickRecoveryPerk : BasePerk
{

    private QuickRecoveryPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (QuickRecoveryPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.FighterRestoredHP += OnFighterHealed;
    }

    public override void OnRemove()
    {
        GameplayEvents.FighterRestoredHP -= OnFighterHealed;
    }

    private void OnDestroy()
    {
        GameplayEvents.FighterRestoredHP -= OnFighterHealed;
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

    private void OnFighterHealed(Fighter fighter, int amount)
    {
        if (GameInfoHelper.CompareFighterToPlayer(fighter))
        {
            fighter.HP.Heal(m_perkData.Restore);
        }
    }
}
