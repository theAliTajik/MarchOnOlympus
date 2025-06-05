using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thornful Block", menuName = "Cards/Thornful Block Card")]
public class ThornfulBlockCard : BaseCardData
{
    public int Block;
    public int StanceDamage;

    protected override Type GetActionType()
    {
        return typeof(ThornfulBlockCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceDamage);
        }
        else
        {
            return string.Format(normalDataSet.description, Block);
        }
    }
}