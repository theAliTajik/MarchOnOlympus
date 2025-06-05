using System;
using Game;
using UnityEngine;

public class GainMinusThirtyHPPerk : BasePerk
{

    private GainMinusThirtyHPPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (GainMinusThirtyHPPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.COMBAT_START};
        return phases;
    }

    public override float GetPriority()
    {
        return 1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.COMBAT_START:
                Fighter player = GameInfoHelper.GetPlayer();
                GameActionHelper.DamageFighter(player, player,  m_perkData.Damage, false, true);
                RemoveSelf();
                break;
        }
    }
}
