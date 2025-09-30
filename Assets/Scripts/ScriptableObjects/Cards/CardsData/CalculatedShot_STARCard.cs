using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CalculatedShot_STAR", menuName = "Cards/CalculatedShot_STARCard")]
public class CalculatedShot_STARCard : BaseCardData
{
    public int Damage;
    public int InventThreshold;
    public int InventDamage;
    
    protected override Type GetActionType()
    {
        return typeof(CalculatedShot_STARCardAction);
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