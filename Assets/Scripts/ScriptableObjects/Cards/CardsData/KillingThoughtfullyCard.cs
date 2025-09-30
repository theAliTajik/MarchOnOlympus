using System;
using UnityEngine;


[CreateAssetMenu(fileName = "KillingThoughtfully", menuName = "Cards/KillingThoughtfullyCard")]
public class KillingThoughtfullyCard : BaseCardData
{
    public int Damage;
    public int Bleed;
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(KillingThoughtfullyCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Bleed, Invent);
        }
    }
}