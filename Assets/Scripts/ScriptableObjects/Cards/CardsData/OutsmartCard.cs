using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Outsmart", menuName = "Cards/OutsmartCard")]
public class OutsmartCard : BaseCardData
{
    public int DrawExtra;
    
    protected override Type GetActionType()
    {
        return typeof(OutsmartCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, DrawExtra);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}