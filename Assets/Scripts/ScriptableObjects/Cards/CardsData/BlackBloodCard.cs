using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BlackBlood", menuName = "Cards/BlackBloodCard")]
public class BlackBloodCard : BaseCardData
{
    public int Str;
    public int Bleed;
    
    
    protected override Type GetActionType()
    {
        return typeof(BlackBloodCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Str, Bleed);
        }
    }
}