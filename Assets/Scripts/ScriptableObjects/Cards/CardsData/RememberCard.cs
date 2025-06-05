using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Remember", menuName = "Cards/RememberCard")]
public class RememberCard : BaseCardData
{
    public int CardReturnAmount;
    public int CardCost;

    public int StanceCardReturnAmount;
    public int StanceCardCost;
    
    protected override Type GetActionType()
    {
        return typeof(RememberCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceCardReturnAmount, StanceCardCost);
        }
        else
        {
            return string.Format(normalDataSet.description, CardReturnAmount, CardCost);
        }
    }
}