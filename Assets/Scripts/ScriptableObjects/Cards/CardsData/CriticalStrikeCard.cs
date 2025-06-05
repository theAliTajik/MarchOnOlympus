using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Critical Strike", menuName = "Cards/Critical Strike Card")]
public class CriticalStrikeCard : BaseCardData
{
    public int Damage;

    protected override Type GetActionType()
    {
        return typeof(CriticalStrikeCardAction);
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