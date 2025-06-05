using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Dual Strike ", menuName = "Cards/Dual Strike Card")]
public class DualStrikeCard : BaseCardData
{
    public int Damage;
    public int NumberOfAttacks;
    public float DelayBetweenAttacks;
    
    protected override Type GetActionType()
    {
        return typeof(DualStrikeCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, NumberOfAttacks);
        }
    }
}