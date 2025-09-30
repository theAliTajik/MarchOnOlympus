using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Overstudy", menuName = "Cards/OverstudyCard")]
public class OverstudyCard : BaseCardData
{
    public int Fortified;
    
    protected override Type GetActionType()
    {
        return typeof(OverstudyCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Fortified);
        }
    }
}