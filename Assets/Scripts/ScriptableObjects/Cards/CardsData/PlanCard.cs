using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Plan", menuName = "Cards/Plan Card")]
public class PlanCard : BaseCardData
{
    public int StanceBlockAmount;
    
    protected override Type GetActionType()
    {
        return typeof(PlanCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceBlockAmount);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}