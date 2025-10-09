using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ImprovisedArrow_STAR", menuName = "Cards/ImprovisedArrow_STARCard")]
public class ImprovisedArrow_STARCard : BaseCardData
{
    public int Strength;
    
    protected override Type GetActionType()
    {
        return typeof(ImprovisedArrow_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Strength);
        }
    }
}