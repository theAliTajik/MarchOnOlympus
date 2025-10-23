using System;
using Game;
using UnityEngine;

public class LessIsMoreXYZPerk : BasePerk
{

    private LessIsMoreXYZPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (LessIsMoreXYZPerkData)perkData;
    }

    public override void OnAdd()
    {
    }

    public override void OnRemove()
    {
    }

    private void OnDestroy()
    {
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return 4;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int numOfCardsPlayed = GameInfoHelper.GetNumOfCardPlayedThisTurn();

        if (numOfCardsPlayed < m_perkData.NumOfCardsThreshold)
        {
            GameActionHelper.AddMechanicToPlayer(m_perkData.Strength, MechanicType.STRENGTH);
        }
        callback?.Invoke();
    }
}
