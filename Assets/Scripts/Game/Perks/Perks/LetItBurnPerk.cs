using System;
using Game;
using UnityEngine;

public class LetItBurnPerk : BasePerk
{
    private LetItBurnPerkData m_perkData;

    private int NumOfPerished;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (LetItBurnPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_PERISHED, EGamePhase.CARD_PLAYED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.CARD_PERISHED:
                NumOfPerished++;
                break;
            case EGamePhase.CARD_PLAYED:
                if (NumOfPerished > 0)
                {
                    NumOfPerished--;
                    Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
                    GameActionHelper.DamageFighter(randEnemy, GameInfoHelper.GetPlayer(), m_perkData.Damage);
                }
                break;
        }
     
    }
}
