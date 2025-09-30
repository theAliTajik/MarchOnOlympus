using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Devise", menuName = "Cards/DeviseCard")]
public class DeviseCard : BaseCardData
{
    public int Damage;
    public int InventDivisor;
    
    protected override Type GetActionType()
    {
        return typeof(DeviseCardAction);
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