using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BerserkersRage", menuName = "Cards/BerserkersRageCard")]
public class BerserkersRageCard : BaseCardData
{
    public int StrGain;
    public int SelfDamage;
    
    protected override Type GetActionType()
    {
        return typeof(BerserkersRageCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, StrGain, SelfDamage);
        }
    }
}