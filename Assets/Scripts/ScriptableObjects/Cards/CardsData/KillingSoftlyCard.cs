using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Killing Softly", menuName = "Cards/Killing Softly Card")]
public class KillingSoftlyCard : BaseCardData
{
    public int ImpaleAmount;
    
    protected override Type GetActionType()
    {
        return typeof(KillingSoftlyCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, ImpaleAmount);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}