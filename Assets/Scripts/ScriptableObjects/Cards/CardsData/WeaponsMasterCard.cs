using System;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponsMaster", menuName = "Cards/WeaponsMasterCard")]
public class WeaponsMasterCard : BaseCardData
{
    public int DamageWhenDrawn;
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(WeaponsMasterCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, DamageWhenDrawn, Damage);
        }
    }
}