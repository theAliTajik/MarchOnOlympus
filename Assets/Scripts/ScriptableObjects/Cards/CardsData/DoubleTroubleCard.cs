using System;
using UnityEngine;


[CreateAssetMenu(fileName = "DoubleTrouble", menuName = "Cards/DoubleTroubleCard")]
public class DoubleTroubleCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(DoubleTroubleCardAction);
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