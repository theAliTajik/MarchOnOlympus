using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Killing Hardly", menuName = "Cards/Killing Hardly Card")]
public class KillingHardlyCard : BaseCardData
{
    public int StanceBleed;
    
    protected override Type GetActionType()
    {
        return typeof(KillingHardlyCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceBleed);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}