using System;
using UnityEngine;


[CreateAssetMenu(fileName = "UntilTheEnd", menuName = "Cards/UntilTheEndCard")]
public class UntilTheEndCard : BaseCardData
{
    public int Restore;
    
    protected override Type GetActionType()
    {
        return typeof(UntilTheEndCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Restore);
        }
    }
}