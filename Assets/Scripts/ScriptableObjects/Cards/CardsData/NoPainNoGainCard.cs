using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "No Pain No Gain", menuName = "Cards/No Pain No Gain Card")]
public class NoPainNoGainCard : BaseCardData
{
    public int StrGain;
    
    protected override Type GetActionType()
    {
        return typeof(NoPainNoGainCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, StrGain);
        }
    }
}