using System;
using Game;
using Game.ModifiableParam;
using UnityEditorInternal;
using UnityEngine;

public class GainDraw3EventPerk : BasePerk
{
    private GainDraw3EventPerkData m_perkData;
    private IParamModifier<int> m_addDraw;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (GainDraw3EventPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_addDraw = new AddValueModifier<int>(m_perkData.ExtraCardDrawAmount);
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
                GameActionHelper.ModifyDrawAmount(m_addDraw);
                break;
            case EGamePhase.PLAYER_TURN_END:
                GameActionHelper.RemoveDrawAmountModifier(m_addDraw);
                RemoveSelf();
                break;
        }
    }
}
