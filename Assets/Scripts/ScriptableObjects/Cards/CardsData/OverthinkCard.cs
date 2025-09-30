using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Overthink", menuName = "Cards/OverthinkCard")]
public class OverthinkCard : BaseCardData
{
    public int InventGain;
    
    protected override Type GetActionType()
    {
        return typeof(OverthinkCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, InventGain);
        }
    }
}