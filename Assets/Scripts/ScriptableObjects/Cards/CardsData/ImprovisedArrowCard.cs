using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ImprovisedArrow", menuName = "Cards/ImprovisedArrowCard")]
public class ImprovisedArrowCard : BaseCardData
{
    public int Damage;
    public int Impale;
    
    protected override Type GetActionType()
    {
        return typeof(ImprovisedArrowCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Impale);
        }
    }
}