using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Combo Strike", menuName = "Cards/Combo Strike Card")]
public class ComboStrikeCard : BaseCardData
{
    public int Damage;
    public int StanceDamage;

    protected override Type GetActionType()
    {
        return typeof(ComboStrikeCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceDamage);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}