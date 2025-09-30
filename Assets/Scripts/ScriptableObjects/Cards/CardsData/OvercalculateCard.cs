using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Overcalculate", menuName = "Cards/OvercalculateCard")]
public class OvercalculateCard : BaseCardData
{
    public int Damage;
    public int InventThreshold;
    public int Daze;
    
    protected override Type GetActionType()
    {
        return typeof(OvercalculateCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, InventThreshold, Daze);
        }
    }
}