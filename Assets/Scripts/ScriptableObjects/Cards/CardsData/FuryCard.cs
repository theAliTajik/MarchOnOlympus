using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Fury", menuName = "Cards/FuryCard")]
public class FuryCard : BaseCardData
{
    public int StrPerTurn;
    public int NumOfTurnsTrigger;
    public int SelfDamage;
    
    protected override Type GetActionType()
    {
        return typeof(FuryCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, StrPerTurn, NumOfTurnsTrigger, SelfDamage);
        }
    }
}