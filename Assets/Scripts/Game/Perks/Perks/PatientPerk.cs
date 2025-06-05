using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PatientPerk : BasePerk
{
    private bool doAddStr = false;
    private int strStack = 1;
    
    
    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED, EGamePhase.PLAYER_TURN_END};
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
            case EGamePhase.PLAYER_TURN_END:
                int energy = GameInfoHelper.GetCurrentEnergy();
                if (energy > 0)
                {
                    doAddStr = true;
                }
                break;
            case EGamePhase.CARD_DRAW_FINISHED:
                if (doAddStr)
                {
                    GameActionHelper.AddMechanicToPlayer(strStack, MechanicType.STRENGTH);
                    doAddStr = false;
                }
                break;
        }
        
        callback?.Invoke();
    }
}
