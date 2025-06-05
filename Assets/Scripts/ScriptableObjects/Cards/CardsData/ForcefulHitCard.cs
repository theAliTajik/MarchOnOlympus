using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Forceful Hit", menuName = "Cards/Forceful Hit Card")]
public class ForcefulHitCard : BaseCardData
{
    public int Damage;
    public int StrMultiplier;

    public int StanceDamage;
    public int StanceStrMultiplier;
    
    protected override Type GetActionType()
    {
        return typeof(ForcefulHitCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceDamage, StanceStrMultiplier);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, StrMultiplier);
        }
    }
}