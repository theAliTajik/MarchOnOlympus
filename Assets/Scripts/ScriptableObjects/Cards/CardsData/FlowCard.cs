using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Flow", menuName = "Cards/FlowCard")]
public class FlowCard : BaseCardData
{
    public int Damage;
    public int DrawAmount;
    
    protected override Type GetActionType()
    {
        return typeof(FlowCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, DrawAmount);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}