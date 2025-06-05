using System;
using UnityEngine;


[CreateAssetMenu(fileName = "DeathWish", menuName = "Cards/DeathWishCard")]
public class DeathWishCard : BaseCardData
{
    public int Damage;
    public int StanceDamage;
    
    protected override Type GetActionType()
    {
        return typeof(DeathWishCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceDamage);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}