using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulwarkCard", menuName = "Cards/Bulwark Card")]
public class BulwarkCard : BaseCardData
{
    public int Block;

    protected override Type GetActionType()
    {
        return typeof(BulwarkCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Block);
        }
    }
}