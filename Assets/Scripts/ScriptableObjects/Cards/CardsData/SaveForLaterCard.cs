using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "SaveForLater", menuName = "Cards/SaveForLaterCard")]
public class SaveForLaterCard : BaseCardData
{
    public int Strength;
    
    protected override Type GetActionType()
    {
        return typeof(SaveForLaterCardAction);
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