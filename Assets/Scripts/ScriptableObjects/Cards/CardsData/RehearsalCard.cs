using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Rehearsal", menuName = "Cards/RehearsalCard")]
public class RehearsalCard : BaseCardData
{
    public int CardReturnAmount;
    public int CostReduction;
    
    public int StanceCardReturnAmount;
    public int StanceCostReduction;
    
    
    protected override Type GetActionType()
    {
        return typeof(RehearsalCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceCardReturnAmount, StanceCostReduction);
        }
        else
        {
            return string.Format(normalDataSet.description, CardReturnAmount, CostReduction);
        }
    }
}