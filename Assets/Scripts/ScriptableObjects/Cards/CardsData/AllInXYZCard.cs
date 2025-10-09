using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "AllInXYZ", menuName = "Cards/AllInXYZCard")]
public class AllInXYZCard : BaseCardData
{
    public int DamageMultiplier;
    
    protected override Type GetActionType()
    {
        return typeof(AllInXYZCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, DamageMultiplier);
        }
    }
}