using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Eviscerate", menuName = "Cards/Eviscerate Card")]
public class EviscerateCard : BaseCardData
{
    public int Damage;
    public int ImpaleAmount;
    
    
    protected override Type GetActionType()
    {
        return typeof(EviscerateCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, ImpaleAmount);
        }
    }
}