using System;
using UnityEngine;


[CreateAssetMenu(fileName = "LastStand", menuName = "Cards/LastStandCard")]
public class LastStandCard : BaseCardData
{
    public int GuardMin;
    public int StanceRestoreNextTurn;
    
    protected override Type GetActionType()
    {
        return typeof(LastStandCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceRestoreNextTurn);
        }
        else
        {
            return string.Format(normalDataSet.description, GuardMin);
        }
    }
}