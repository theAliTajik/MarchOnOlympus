using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PoisonArrow_STAR", menuName = "Cards/PoisonArrow_STARCard")]
public class PoisonArrow_STARCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(PoisonArrow_STARCardAction);
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