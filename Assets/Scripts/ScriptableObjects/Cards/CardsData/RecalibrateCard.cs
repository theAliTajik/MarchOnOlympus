using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Recalibrate", menuName = "Cards/RecalibrateCard")]
public class RecalibrateCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(RecalibrateCardAction);
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