using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Think", menuName = "Cards/ThinkCard")]
public class ThinkCard : BaseCardData
{
    public int InventGain;
    
    protected override Type GetActionType()
    {
        return typeof(ThinkCardAction);
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