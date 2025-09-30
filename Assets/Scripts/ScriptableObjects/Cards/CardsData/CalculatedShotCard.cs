using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CalculatedShot", menuName = "Cards/CalculatedShotCard")]
public class CalculatedShotCard : BaseCardData
{
    public int Damage;
    public int InventThreshold;
    public int InventDamage;
    
    protected override Type GetActionType()
    {
        return typeof(CalculatedShotCardAction);
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