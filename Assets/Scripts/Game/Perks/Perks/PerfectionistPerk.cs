using System;
using Game;
using UnityEngine;

public class PerfectionistPerk : BasePerk
{

    private PerfectionistPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (PerfectionistPerkData)perkData;
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
        switch (phase)
        {
            case EGamePhase.PLAYER_TURN_START:
                int damageDoneToPlayer = GameInfoHelper.GetDamageDoneToPlayerThisTurn();
                if (damageDoneToPlayer <= 0)
                {
                    GameActionHelper.AddMechanicToPlayer(m_perkData.StrGain, MechanicType.STRENGTH);
                }
                break;
        }
    }
}
