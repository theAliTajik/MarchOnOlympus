using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tempering", menuName = "Cards/Tempering Card")]
public class TemperingCard : BaseCardData
{
    public int Block;
    public int StanceStr;

    protected override Type GetActionType()
    {
        return typeof(TemperingCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceStr);
        }
        else
        {
            return string.Format(normalDataSet.description, Block);
        }
    }
}