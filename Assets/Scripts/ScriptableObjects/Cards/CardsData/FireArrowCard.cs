using System;
using UnityEngine;


[CreateAssetMenu(fileName = "FireArrow", menuName = "Cards/FireArrowCard")]
public class FireArrowCard : BaseCardData
{
    public int Damage;
    public int Burn;
    
    protected override Type GetActionType()
    {
        return typeof(FireArrowCardAction);
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