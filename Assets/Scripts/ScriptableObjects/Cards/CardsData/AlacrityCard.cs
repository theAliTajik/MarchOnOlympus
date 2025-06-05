using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Alacrity", menuName = "Cards/Alacrity Card")]
public class AlacrityCard : BaseCardData
{
    public int Block;
    public int DrawAmount;
    
    protected override Type GetActionType()
    {
        return typeof(AlacrityCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, DrawAmount);
        }
        else
        {
            return string.Format(normalDataSet.description, Block);
        }
    }
}