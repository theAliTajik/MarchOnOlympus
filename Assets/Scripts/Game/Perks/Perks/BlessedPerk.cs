using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class BlessedPerk : BasePerk
{

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};
        return phases;
    }

    public override float GetPriority()
    {
        return 1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.PLAYER_TURN_END:
                List<MechanicType> types = GameInfoHelper.GetDamageOverTimeMechanics();
                foreach (MechanicType t in types)
                {
                    GameActionHelper.RemoveMechanicFromPlayer(t);
                }
                break;
        }
        callback?.Invoke();
    }
}
