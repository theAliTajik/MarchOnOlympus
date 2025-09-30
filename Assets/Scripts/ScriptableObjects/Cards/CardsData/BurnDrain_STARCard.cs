using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BurnDrain_STAR", menuName = "Cards/BurnDrain_STARCard")]
public class BurnDrain_STARCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(BurnDrain_STARCardAction);
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