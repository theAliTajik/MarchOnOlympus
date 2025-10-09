using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ImprovisedBomb_STAR", menuName = "Cards/ImprovisedBomb_STARCard")]
public class ImprovisedBomb_STARCard : BaseCardData
{
    public int Damage;
    public int Vulnerable;
    
    protected override Type GetActionType()
    {
        return typeof(ImprovisedBomb_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Vulnerable);
        }
    }
}