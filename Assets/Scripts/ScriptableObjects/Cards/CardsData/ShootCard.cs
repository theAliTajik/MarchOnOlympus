using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Shoot", menuName = "Cards/ShootCard")]
public class ShootCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(ShootCardAction);
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