using System;
using Game;
using UnityEngine;

public class TalosPerk : BasePerk
{

    private TalosPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (TalosPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DISCARDED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        GameActionHelper.DamageFighter(randEnemy, GameInfoHelper.GetPlayer(), m_perkData.Damage);
    }
}
