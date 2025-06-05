using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Penetrate", menuName = "Cards/Penetrate Card")]
public class PenetrateCard : BaseCardData
{
    public int Multiplier;
    
    protected override Type GetActionType()
    {
        return typeof(PenetrateCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Multiplier);
        }
    }
}