using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "BurnDrain_PLUS", menuName = "Cards/BurnDrain_PLUSCard")]
public class BurnDrain_PLUSCard : BaseCardData
{
    public int ExtraBleed;
    
    protected override Type GetActionType()
    {
        return typeof(BurnDrain_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ExtraBleed);
        }
    }
}