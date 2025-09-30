using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Oversmart_STAR", menuName = "Cards/Oversmart_STARCard")]
public class Oversmart_STARCard : BaseCardData
{
    public int Fortified;
    public int Restore;
    public int Strength;
    
    
    protected override Type GetActionType()
    {
        return typeof(Oversmart_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Fortified, Restore, Strength);
        }
    }
}