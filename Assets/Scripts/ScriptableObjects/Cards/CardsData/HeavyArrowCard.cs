using System;
using UnityEngine;


[CreateAssetMenu(fileName = "HeavyArrow", menuName = "Cards/HeavyArrowCard")]
public class HeavyArrowCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(HeavyArrowCardAction);
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