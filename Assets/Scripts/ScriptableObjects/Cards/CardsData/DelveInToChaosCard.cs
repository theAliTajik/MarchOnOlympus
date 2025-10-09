using System;
using UnityEngine;


[CreateAssetMenu(fileName = "DelveInToChaos", menuName = "Cards/DelveInToChaosCard")]
public class DelveInToChaosCard : BaseCardData
{
    public int PerishCards;
    public int NumOfLegendaryCards;
    
    protected override Type GetActionType()
    {
        return typeof(DelveInToChaosCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, PerishCards, NumOfLegendaryCards);
        }
    }
}