using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Smash", menuName = "Cards/Smash Card")]
public class SmashCard : BaseCardData
{
    public int DamageForEachBuff;
    public int StanceVulnerableAmount;
    
    protected override Type GetActionType()
    {
        return typeof(SmashCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceVulnerableAmount);
        }
        else
        {
            return string.Format(normalDataSet.description, DamageForEachBuff);
        }
    }
}