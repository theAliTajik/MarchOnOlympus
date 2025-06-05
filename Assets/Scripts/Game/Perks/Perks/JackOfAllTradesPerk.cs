using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class JackOfAllTradesPerk : BasePerk
{
    public List<Stance> StancesInOneTurn = new List<Stance>();
    
    private JackOfAllTradesPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (JackOfAllTradesPerkData)perkData;
    }

    public override void OnAdd()
    {
        OnPhaseActivate(EGamePhase.STANCE_CHANGED, null);
    }

    public override void OnRemove()
    {
    }

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.STANCE_CHANGED, EGamePhase.PLAYER_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return 2;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.PLAYER_TURN_START:
                AddStance();
                break;
            case EGamePhase.STANCE_CHANGED:
                AddStance();
                break;
            
            case EGamePhase.PLAYER_TURN_END:
                StancesInOneTurn.Clear();
                break;
        }
    }

    private void AddStance()
    {
        Stance CurrentStance = GameInfoHelper.GetCurrentStance();
        if (StancesInOneTurn.Contains(CurrentStance) || CurrentStance == Stance.NONE)
        {
            return;
        }
        StancesInOneTurn.Add(CurrentStance);
        int AllStancesCount = Enum.GetValues(typeof(Stance)).Length;
        AllStancesCount--; // remove the NONE stance

        if (StancesInOneTurn.Count >= AllStancesCount)
        {
            Apply();
        }
    }

    public void Apply()
    {
        GameActionHelper.AddMechanicToPlayer(m_perkData.StrGain, MechanicType.STRENGTH);
        GameActionHelper.AddMechanicToPlayer(m_perkData.BlockGain, MechanicType.BLOCK);
        GameActionHelper.DrawCards(m_perkData.DrawExtra);
    }
}
