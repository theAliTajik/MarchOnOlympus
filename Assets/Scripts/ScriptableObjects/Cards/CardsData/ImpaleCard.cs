using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Impale", menuName = "Cards/Impale Card")]
public class ImpaleCard : BaseCardData
{
    public int Damage;
    public int ImpaleAmount;
    
    
    protected override Type GetActionType()
    {
        return typeof(ImpaleCardAction);
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