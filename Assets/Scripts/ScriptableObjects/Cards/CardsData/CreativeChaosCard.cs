using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CreativeChaos", menuName = "Cards/CreativeChaosCard")]
public class CreativeChaosCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(CreativeChaosCardAction);
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