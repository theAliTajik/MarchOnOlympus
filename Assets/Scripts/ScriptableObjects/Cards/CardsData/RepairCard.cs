using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair", menuName = "Cards/Repair Card")]
public class RepairCard : BaseCardData
{
    protected override Type GetActionType()
    {
        return typeof(RepairCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return normalDataSet.description;
        }
    }
}