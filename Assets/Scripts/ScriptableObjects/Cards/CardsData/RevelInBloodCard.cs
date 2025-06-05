using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Revel In Blood", menuName = "Cards/Revel In Blood Card")]
public class RevelInBloodCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(RevelInBloodCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}