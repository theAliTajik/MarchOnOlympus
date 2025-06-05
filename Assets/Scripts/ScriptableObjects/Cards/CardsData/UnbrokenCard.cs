using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Unbroken", menuName = "Cards/UnbrokenCard")]
public class UnbrokenCard : BaseCardData
{
    public int StrGain;
    
    protected override Type GetActionType()
    {
        return typeof(UnbrokenCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, StrGain);
        }
    }
}