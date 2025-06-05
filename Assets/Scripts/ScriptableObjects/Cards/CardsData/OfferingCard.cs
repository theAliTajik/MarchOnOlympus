using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Offering", menuName = "Cards/OfferingCard")]
public class OfferingCard : BaseCardData
{
    public int CostDamageMultiplier;
    public int CostThreshold;
    public BaseCardData CardToSpawn;
    
    protected override Type GetActionType()
    {
        return typeof(OfferingCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, name);
        }
        else
        {
            return string.Format(normalDataSet.description, name);
        }
    }
}