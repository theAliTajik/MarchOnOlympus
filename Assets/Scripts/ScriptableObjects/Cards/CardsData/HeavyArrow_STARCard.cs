using System;
using UnityEngine;


[CreateAssetMenu(fileName = "HeavyArrow_STAR", menuName = "Cards/HeavyArrow_STARCard")]
public class HeavyArrow_STARCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(HeavyArrow_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}