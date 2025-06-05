using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CookBook", menuName = "Cards/CookBookCard")]
public class CookBookCard : BaseCardData
{
    public int CardCostOverride;
    
    protected override Type GetActionType()
    {
        return typeof(CookBookCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, CardCostOverride);
        }
    }
}