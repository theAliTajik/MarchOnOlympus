using System;
using Game;
using UnityEngine;

public class LoneWolfPerk : BasePerk
{

    private LoneWolfPerkData m_perkData;

    private bool IsConditionApplied;
    private bool IsConditionTrue;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (LoneWolfPerkData)perkData;
    }

    public override void OnAdd()
    {
        OnPhaseActivate(EGamePhase.ENEMY_TURN_START, null);
    }

    public override void OnRemove()
    {
        if (IsConditionApplied)
        {
            GameActionHelper.DecreasePlayerMaxHP(m_perkData.ExtraMaxHP);
        }
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_START};
        return phases;
    }

    public override float GetPriority()
    {
        return 12;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int NumOfCardsInDeck = GameInfoHelper.CountNumOfCardsInDeck(CardStorage.ALL);
        //check condition
        if (NumOfCardsInDeck == m_perkData.NumOfCardsTrigger)
        {
            IsConditionTrue = true;
        }
        else
        {
            IsConditionTrue = false;
        }
        
        // if condition true and not applied apply
        if (IsConditionTrue && !IsConditionApplied)
        {
            GameActionHelper.IncreasePlayerMaxHP(m_perkData.ExtraMaxHP);
            IsConditionApplied = true;
        }

        // if condition not true but has been applied reverse effects
        if (!IsConditionTrue && IsConditionApplied)
        {
            GameActionHelper.DecreasePlayerMaxHP(m_perkData.ExtraMaxHP);
            IsConditionApplied = false;
        }

        //if condition true gain block
        if (IsConditionTrue)
        {
            GameActionHelper.AddMechanicToPlayer(m_perkData.BlockGain, MechanicType.BLOCK);
        }
    }
}
