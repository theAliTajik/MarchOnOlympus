using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PoisonArrow", menuName = "Cards/PoisonArrowCard")]
public class PoisonArrowCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(PoisonArrowCardAction);
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