using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Rend", menuName = "Cards/Rend Card")]
public class RendCard : BaseCardData
{
    public int Bleed;
    public int StanceManaDecrease;
    
    protected override Type GetActionType()
    {
        return typeof(RendCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceManaDecrease);
        }
        else
        {
            return string.Format(normalDataSet.description, Bleed);
        }
    }
}