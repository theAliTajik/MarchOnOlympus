using System;
using Game;
using UnityEngine;

public class MoreTheMerrierPerk : BasePerk
{

    private MoreTheMerrierPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (MoreTheMerrierPerkData)perkData;
    }

    public override void OnAdd()
    {
        m_perkData.HasIncreasedDraw = false;
    }

    public override void OnRemove()
    {
        if (m_perkData.HasIncreasedDraw)
        {
            GameActionHelper.DecreaseDrawAmount(m_perkData.DrawAmount);
        }
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return 2;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int numOfEnemies = GameInfoHelper.GetNumOfEnemies();
        if (numOfEnemies > m_perkData.NumOfEnemies)
        {
            m_perkData.HasIncreasedDraw = true;
            GameActionHelper.IncreaseDrawAmount(m_perkData.DrawAmount);
        }
    }
}
