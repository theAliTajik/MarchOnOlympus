using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PiercingArrow_STAR", menuName = "Cards/PiercingArrow_STARCard")]
public class PiercingArrow_STARCard : BaseCardData
{
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(PiercingArrow_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Bleed);
        }
    }
}