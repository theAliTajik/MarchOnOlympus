using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Thirst For Blood", menuName = "Cards/Thirst For Blood Card")]
public class ThirstForBloodCard : BaseCardData
{
    public int Damage;
    public int ExtraDamageIfTargetBleeding;

    public int StanceRestoreHP;
    protected override Type GetActionType()
    {
        return typeof(ThirstForBloodCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceRestoreHP);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, ExtraDamageIfTargetBleeding);
        }
    }
}