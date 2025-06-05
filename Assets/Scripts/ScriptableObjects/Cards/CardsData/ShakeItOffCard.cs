using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ShakeItOff", menuName = "Cards/ShakeItOffCard")]
public class ShakeItOffCard : BaseCardData
{
    public int RestoreForEachDebuff;
    
    protected override Type GetActionType()
    {
        return typeof(ShakeItOffCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, RestoreForEachDebuff);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}