using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Boomerang_PLUS", menuName = "Cards/Boomerang_PLUSCard")]
public class Boomerang_PLUSCard : BaseCardData
{
    public int Damage;
    public int DamageIncrease;
    
    protected override Type GetActionType()
    {
        return typeof(Boomerang_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, DamageIncrease);
        }
    }
}