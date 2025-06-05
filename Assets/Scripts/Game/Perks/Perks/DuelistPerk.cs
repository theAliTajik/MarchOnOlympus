using System;
using Game;
using UnityEngine;

public class DuelistPerk : BasePerk
{

    private DuelistPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (DuelistPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int enemiesCount = GameInfoHelper.GetNumOfEnemies();
        if (enemiesCount <= m_perkData.NumOfEnemies)
        {
            GameActionHelper.AddMechanicToPlayer(m_perkData.BlockGain, MechanicType.BLOCK);
        }
    }
}
