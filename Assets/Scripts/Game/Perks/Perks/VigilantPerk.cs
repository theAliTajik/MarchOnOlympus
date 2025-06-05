using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class VigilantPerk : BasePerk
{

    private int m_drawAmountIncrease = 1;
    
    public override void OnAdd()
    {
        GameActionHelper.IncreaseDrawAmount(m_drawAmountIncrease);
    }

    public override void OnRemove()
    {
        GameActionHelper.IncreaseDrawAmount(-m_drawAmountIncrease);
    }

    public override EGamePhase[] GetPhases()
    {
        return null;
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        throw new NotImplementedException();
    }
}
