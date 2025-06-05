using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Blessing", menuName = "Cards/Blessing Card")]
public class BlessingCard : BaseCardData
{
    public int RestoreAmount;
    
    protected override Type GetActionType()
    {
        return typeof(BlessingCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, RestoreAmount);
        }
    }
}