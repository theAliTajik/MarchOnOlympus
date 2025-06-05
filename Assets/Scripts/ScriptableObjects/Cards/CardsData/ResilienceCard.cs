using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Resilience", menuName = "Cards/ResilienceCard")]
public class ResilienceCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(ResilienceCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return normalDataSet.description;
        }
    }
}