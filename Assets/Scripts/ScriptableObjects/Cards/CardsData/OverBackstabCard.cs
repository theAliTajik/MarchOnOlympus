using System;
using UnityEngine;


[CreateAssetMenu(fileName = "OverBackstab", menuName = "Cards/OverBackstabCard")]
public class OverBackstabCard : BaseCardData
{
    public int Damage;
    public int Energy;
    
    protected override Type GetActionType()
    {
        return typeof(OverBackstabCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Energy);
        }
    }
}