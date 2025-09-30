using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Honorable", menuName = "Cards/HonorableCard")]
public class HonorableCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(HonorableCardAction);
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