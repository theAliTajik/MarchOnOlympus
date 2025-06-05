using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Ready", menuName = "Cards/ReadyCard")]
public class ReadyCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(ReadyCardAction);
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