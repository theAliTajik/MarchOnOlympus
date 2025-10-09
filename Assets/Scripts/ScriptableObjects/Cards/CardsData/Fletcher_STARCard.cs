using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Fletcher_STAR", menuName = "Cards/Fletcher_STARCard")]
public class Fletcher_STARCard : BaseCardData
{
    public int ExtraDamage;
    
    protected override Type GetActionType()
    {
        return typeof(Fletcher_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ExtraDamage);
        }
    }
}