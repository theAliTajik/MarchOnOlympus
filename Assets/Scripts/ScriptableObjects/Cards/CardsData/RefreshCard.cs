using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Refresh", menuName = "Cards/RefreshCard")]
public class RefreshCard : BaseCardData
{
    protected override Type GetActionType()
    {
        return typeof(RefreshCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return normalDataSet.description;
        }
    }
}