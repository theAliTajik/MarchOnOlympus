using System;
using Game;
using UnityEngine;

public class JoyOfSerenityPerk : BasePerk
{

    private JoyOfSerenityPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (JoyOfSerenityPerkData)perkData;
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
        return 6;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        Fighter player = GameInfoHelper.GetPlayer();
        int playerFortified = GameInfoHelper.GetMechanicStack(player, m_perkData.MechanicType);
        if (playerFortified > 0)
        {
            GameActionHelper.HealPlayer(playerFortified * m_perkData.Restore);
        }
    }
}
