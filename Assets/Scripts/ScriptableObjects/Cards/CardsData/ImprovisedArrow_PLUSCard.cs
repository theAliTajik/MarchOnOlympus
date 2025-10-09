using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ImprovisedArrow_PLUS", menuName = "Cards/ImprovisedArrow_PLUSCard")]
public class ImprovisedArrow_PLUSCard : BaseCardData
{
    public int Damage;
    public int Impale;
    
    protected override Type GetActionType()
    {
        return typeof(ImprovisedArrow_PLUSCardAction);
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