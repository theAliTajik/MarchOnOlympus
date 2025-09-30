using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BurningBash_STAR", menuName = "Cards/BurningBash_STARCard")]
public class BurningBash_STARCard : BaseCardData
{
    public int BurnMultiplier;
    
    protected override Type GetActionType()
    {
        return typeof(BurningBash_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, BurnMultiplier);
        }
    }
}