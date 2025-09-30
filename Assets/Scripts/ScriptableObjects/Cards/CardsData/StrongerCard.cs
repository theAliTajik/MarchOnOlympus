using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Stronger", menuName = "Cards/StrongerCard")]
public class StrongerCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(StrongerCardAction);
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