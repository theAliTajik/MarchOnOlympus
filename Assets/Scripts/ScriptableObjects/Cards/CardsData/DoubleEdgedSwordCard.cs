using System;
using UnityEngine;


[CreateAssetMenu(fileName = "DoubleEdgedSword", menuName = "Cards/DoubleEdgedSwordCard")]
public class DoubleEdgedSwordCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(DoubleEdgedSwordCardAction);
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