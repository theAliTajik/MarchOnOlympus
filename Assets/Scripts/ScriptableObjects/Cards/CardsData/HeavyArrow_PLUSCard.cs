using System;
using UnityEngine;


[CreateAssetMenu(fileName = "HeavyArrow_PLUS", menuName = "Cards/HeavyArrow_PLUSCard")]
public class HeavyArrow_PLUSCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(HeavyArrow_PLUSCardAction);
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