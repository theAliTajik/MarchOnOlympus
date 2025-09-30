using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BurningBlood", menuName = "Cards/BurningBloodCard")]
public class BurningBloodCard : BaseCardData
{
    protected override Type GetActionType()
    {
        return typeof(BurningBloodCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return normalDataSet.description;
        }
    }
}