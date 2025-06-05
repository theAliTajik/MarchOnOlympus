using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Optimize", menuName = "Cards/OptimizeCard")]
public class OptimizeCard : BaseCardData
{
    public int CostMultiplierDamage;
    public int StanceCostMultiplierDamage;
    
    
    protected override Type GetActionType()
    {
        return typeof(OptimizeCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceCostMultiplierDamage);
        }
        else
        {
            return string.Format(normalDataSet.description, CostMultiplierDamage);
        }
    }
}