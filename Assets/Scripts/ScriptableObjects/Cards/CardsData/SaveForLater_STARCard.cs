using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SaveForLater_STAR", menuName = "Cards/SaveForLater_STARCard")]
public class SaveForLater_STARCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(SaveForLater_STARCardAction);
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