using System;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Exhaust", menuName = "Cards/ExhaustCard")]
public class ExhaustCard : BaseCardData
{
    public int InventLevelMultiplier;
    
    protected override Type GetActionType()
    {
        return typeof(ExhaustCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, InventLevelMultiplier);
        }
    }
}