using System;
using UnityEngine;


[CreateAssetMenu(fileName = "FireArrow_PLUS", menuName = "Cards/FireArrow_PLUSCard")]
public class FireArrow_PLUSCard : BaseCardData
{
    public int Damage;
    public int Burn;
    
    protected override Type GetActionType()
    {
        return typeof(FireArrow_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Burn);
        }
    }
}