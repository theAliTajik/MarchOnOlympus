using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class BloodthirstPerk : BasePerk
{

    private BloodthirstPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (BloodthirstPerkData)perkData;
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
        List<Fighter> enemies = GameInfoHelper.GetAllEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            bool hasBleed = GameInfoHelper.CheckIfFighterHasMechanic(enemies[i], m_perkData.mechanicType);
            if (hasBleed)
            {
                GameActionHelper.HealPlayer(m_perkData.Restore);
            }
        }
    }
}
