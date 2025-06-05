using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Improvise", menuName = "Cards/ImproviseCard")]
public class ImproviseCard : BaseCardData
{
    public int CardSpawn;
    public int CardCostOverride;
    public int StanceCardSpawn;
    public int StanceCardCostOverride;
    
    protected override Type GetActionType()
    {
        return typeof(ImproviseCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceCardSpawn, StanceCardCostOverride);
        }
        else
        {
            return string.Format(normalDataSet.description, CardSpawn, CardCostOverride);
        }
    }
}