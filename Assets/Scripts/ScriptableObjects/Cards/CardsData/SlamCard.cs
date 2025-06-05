using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Slam Card", menuName = "Cards/Slam Card")]
public class SlamCard : BaseCardData
{
    public int block;

    protected override Type GetActionType()
    {
        return typeof(SlamCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, block);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}
