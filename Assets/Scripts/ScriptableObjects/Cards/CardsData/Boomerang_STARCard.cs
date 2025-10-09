using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Boomerang_STAR", menuName = "Cards/Boomerang_STARCard")]
public class Boomerang_STARCard : BaseCardData
{
    public int Damage;
    public int DamageIncrease;
    
    protected override Type GetActionType()
    {
        return typeof(Boomerang_STARCardAction);
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