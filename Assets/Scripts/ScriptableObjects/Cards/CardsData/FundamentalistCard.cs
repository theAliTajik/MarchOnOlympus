using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Fundamentalist", menuName = "Cards/Fundamentalist Card")]
public class FundamentalistCard : BaseCardData
{
    public int BoostAmount;
    public int StanceBoostAmount;
    
    public string DescriptionToAddToCards;
    
    protected override Type GetActionType()
    {
        return typeof(FundamentalistCardAction);
    }
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceBoostAmount);
        }
        else
        {
            return string.Format(normalDataSet.description, BoostAmount);
        }
    }
}