using System;
using Game;
using UnityEngine;

public class InArmorWeTrustPerk : BasePerk
{

    private InArmorWeTrustPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (InArmorWeTrustPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_START};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        Fighter player = GameInfoHelper.GetPlayer();
        int strStack = GameInfoHelper.GetMechanicStack(player, MechanicType.STRENGTH);
        if (strStack > 0)
        {
            GameActionHelper.AddMechanicToPlayer(strStack, m_perkData.MechanicType);
        }
    }
}
