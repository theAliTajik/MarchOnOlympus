using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Shuffle", menuName = "Cards/ShuffleCard")]
public class ShuffleCard : BaseCardData
{
    public int CardCostReduction;
    
    protected override Type GetActionType()
    {
        return typeof(ShuffleCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, CardCostReduction);
        }
    }
}