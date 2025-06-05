using System;
using UnityEngine;


[CreateAssetMenu(fileName = "IgnorePain", menuName = "Cards/IgnorePainCard")]
public class IgnorePainCard : BaseCardData
{
    public int Block;
    
    protected override Type GetActionType()
    {
        return typeof(IgnorePainCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, Block);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}