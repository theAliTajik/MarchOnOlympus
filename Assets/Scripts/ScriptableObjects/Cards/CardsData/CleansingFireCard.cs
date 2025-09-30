using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "CleansingFire", menuName = "Cards/CleansingFireCard")]
public class CleansingFireCard : BaseCardData
{
    public int SelfBurn;
    public int Restore;
    
    protected override Type GetActionType()
    {
        return typeof(CleansingFireCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, SelfBurn, Restore);
        }
    }
}