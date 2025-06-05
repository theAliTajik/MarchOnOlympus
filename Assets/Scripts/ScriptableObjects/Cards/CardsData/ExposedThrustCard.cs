using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Exposed Thrust", menuName = "Cards/Exposed Thrust Card")]
public class ExposedThrustCard : BaseCardData
{
    public int Damage;
    public int ImpaleAmount;
    
    protected override Type GetActionType()
    {
        return typeof(ExposedThrustCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, ImpaleAmount);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}