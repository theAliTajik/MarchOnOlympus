using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "CaltropsXYZ", menuName = "Cards/CaltropsXYZCard")]
public class CaltropsXYZCard : BaseCardData
{
    public int Block;
    public int Thorns;
    
    protected override Type GetActionType()
    {
        return typeof(CaltropsXYZCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Block, Thorns);
        }
    }
}