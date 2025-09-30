using System;
using UnityEngine;


[CreateAssetMenu(fileName = "MakeThemBleedMore", menuName = "Cards/MakeThemBleedMoreCard")]
public class MakeThemBleedMoreCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(MakeThemBleedMoreCardAction);
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