using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "SaveForLater_STAR", menuName = "Cards/SaveForLater_STARCard")]
public class SaveForLater_STARCard : BaseCardData
{
    public int Strength;
    
    protected override Type GetActionType()
    {
        return typeof(SaveForLater_STARCardAction);
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