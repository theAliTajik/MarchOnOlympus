using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Ponder", menuName = "Cards/PonderCard")]
public class PonderCard : BaseCardData
{
    public int CardDrawAmount;
    public int CardCost;
    
    
    protected override Type GetActionType()
    {
        return typeof(PonderCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, CardCost);
        }
    }
}