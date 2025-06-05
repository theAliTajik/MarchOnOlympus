using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Master", menuName = "Cards/Master Card")]
public class MasterCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(MasterCardAction);
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