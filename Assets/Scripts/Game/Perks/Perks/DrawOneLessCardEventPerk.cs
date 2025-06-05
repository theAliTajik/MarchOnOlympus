using System;
using Game;
using Game.ModifiableParam;
using UnityEngine;

public class DrawOneLessCardEventPerk : BasePerk
{
    
    private DrawOneLessCardEventPerkData m_perkData;
    private IParamModifier<int> m_reduceDraw;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (DrawOneLessCardEventPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_reduceDraw = new AddValueModifier<int>(-m_perkData.ReduceAmount);
        OnPhaseActivate(EGamePhase.COMBAT_START, null);
    }
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.COMBAT_START:
                GameActionHelper.ModifyDrawAmount(m_reduceDraw);
                break;
            case EGamePhase.PLAYER_TURN_END:
                GameActionHelper.RemoveDrawAmountModifier(m_reduceDraw);
                RemoveSelf();
                break;
        }
    }}
