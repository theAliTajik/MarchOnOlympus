using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Eureka", menuName = "Cards/EurekaCard")]
public class EurekaCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(EurekaCardAction);
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