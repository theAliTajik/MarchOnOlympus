using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Hector", menuName = "Cards/HectorCard")]
public class HectorCard : BaseCardData
{
    public int Restore;
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(HectorCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Restore, Bleed);
        }
    }
}