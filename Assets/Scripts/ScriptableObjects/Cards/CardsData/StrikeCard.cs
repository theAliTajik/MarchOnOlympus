using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "New Strike Card", menuName = "Cards/Strike Card")]
public class StrikeCard : BaseCardData
{
    public int Damage;
    public int Bleed;

    protected override Type GetActionType()
    {
        return typeof(StrikeCardAction);
    }

    public override string GetDescription(bool Stance = false)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, Bleed);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}
