using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Boom", menuName = "Cards/BoomCard")]
public class BoomCard : BaseCardData
{
    public int Damage;
    public int Burn;
    
    protected override Type GetActionType()
    {
        return typeof(BoomCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Burn);
        }
    }
}