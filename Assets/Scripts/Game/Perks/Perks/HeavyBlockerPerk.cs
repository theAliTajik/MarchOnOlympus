using System;
using Game;
using UnityEngine;

public class HeavyBlockerPerk : BasePerk
{

    private HeavyBlockerPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (HeavyBlockerPerkData)perkData;
    }

    public override void OnAdd()
    {
        Fighter player = GameInfoHelper.GetPlayer();
        GameActionHelper.SetFighterMaxDamagePercentage(player, m_perkData.MaxDamagePercentage);
    }

    public override void OnRemove()
    {
        Fighter player = GameInfoHelper.GetPlayer();
        GameActionHelper.RemoveFighterMaxDamagePercentage(player);
    }
    
    private void OnDestroy(){}

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
}
