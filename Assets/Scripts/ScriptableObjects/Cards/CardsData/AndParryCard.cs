using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "And Parry", menuName = "Cards/And Parry Card")]
public class AndParryCard : BaseCardData
{
    
    
    protected override Type GetActionType()
    {
        return typeof(AndParryCardAction);
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