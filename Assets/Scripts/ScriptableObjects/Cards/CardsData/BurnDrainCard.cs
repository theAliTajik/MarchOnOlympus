using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BurnDrain", menuName = "Cards/BurnDrainCard")]
public class BurnDrainCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(BurnDrainCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description);
        }
    }
}