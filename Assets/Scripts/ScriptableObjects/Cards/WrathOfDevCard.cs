using System;
using UnityEngine;


[CreateAssetMenu(fileName = "WrathOfDev", menuName = "Cards/WrathOfDevCard")]
public class WrathOfDevCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(WrathOfDevCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, Damage);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}