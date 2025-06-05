using System;
using Game;
using UnityEngine;

public class MeticulousPerk : BasePerk
{

    private MeticulousPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (MeticulousPerkData)perkData;
    }

    public override void OnAdd(){}

    public override void OnRemove()
    {
        GameActionHelper.DecreaseDrawAmount(m_perkData.CardDraw);
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.ENEMY_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return 1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int playerHpPercentage = GameInfoHelper.GetPlayerHPPrecentage();
        if (playerHpPercentage > m_perkData.AboveHPPrecent)
        {
            GameActionHelper.IncreaseDrawAmount(m_perkData.CardDraw);
        }
    }
}
