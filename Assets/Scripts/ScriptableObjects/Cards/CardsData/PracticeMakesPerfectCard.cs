using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PracticeMakesPerfect", menuName = "Cards/PracticeMakesPerfectCard")]
public class PracticeMakesPerfectCard : BaseCardData
{
    public int Damage;
    public int DamageIncrease;

    public int DamageIncreaseOnKillingBlow;
    
    protected override Type GetActionType()
    {
        return typeof(PracticeMakesPerfectCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, DamageIncreaseOnKillingBlow);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, DamageIncrease);
        }
    }
}