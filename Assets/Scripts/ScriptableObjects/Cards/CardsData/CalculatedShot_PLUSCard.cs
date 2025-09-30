using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CalculatedShot_PLUS", menuName = "Cards/CalculatedShot_PLUSCard")]
public class CalculatedShot_PLUSCard : BaseCardData
{
    public int Damage;
    public int InventThreshold;
    public int InventDamage;
    
    protected override Type GetActionType()
    {
        return typeof(CalculatedShot_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, InventThreshold, InventDamage);
        }
    }
}