using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PoisonArrow_PLUS", menuName = "Cards/PoisonArrow_PLUSCard")]
public class PoisonArrow_PLUSCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(PoisonArrow_PLUSCardAction);
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