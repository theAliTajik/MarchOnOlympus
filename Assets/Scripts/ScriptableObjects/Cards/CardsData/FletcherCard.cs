using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Fletcher", menuName = "Cards/FletcherCard")]
public class FletcherCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(FletcherCardAction);
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