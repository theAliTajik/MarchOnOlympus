using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Backstab", menuName = "Cards/BackstabCard")]
public class BackstabCard : BaseCardData
{
    public int Damage;
    public int NormalCost;
    public int CostIfAnyTargetHasADebuff;
    
    protected override Type GetActionType()
    {
        stanceDataSet.EnergyCost = NormalCost;
        return typeof(BackstabCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, CostIfAnyTargetHasADebuff);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}