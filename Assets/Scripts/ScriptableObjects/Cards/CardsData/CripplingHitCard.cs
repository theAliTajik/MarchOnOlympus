using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Crippling Hit", menuName = "Cards/Crippling Hit Card")]
public class CripplingHitCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(CripplingHitCardAction);
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