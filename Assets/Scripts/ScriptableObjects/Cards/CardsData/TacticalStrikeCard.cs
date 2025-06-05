using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Tactical Strike", menuName = "Cards/Tactical Strike Card")]
public class TacticalStrikeCard : BaseCardData
{
    public int Damage;
    public int StanceExtraManaCost;
    
    
    protected override Type GetActionType()
    {
        return typeof(TacticalStrikeCardAction);
    }

    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceExtraManaCost);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}