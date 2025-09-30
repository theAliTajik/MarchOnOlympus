using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "BurningBash_PLUS", menuName = "Cards/BurningBash_PLUSCard")]
public class BurningBash_PLUSCard : BaseCardData
{
    public int BurnMultiplier;
    
    protected override Type GetActionType()
    {
        return typeof(BurningBash_PLUSCardAction);
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