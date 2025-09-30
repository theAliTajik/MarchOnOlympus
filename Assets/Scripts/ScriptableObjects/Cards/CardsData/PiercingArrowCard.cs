using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PiercingArrow", menuName = "Cards/PiercingArrowCard")]
public class PiercingArrowCard : BaseCardData
{
    public int Damage;
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(PiercingArrowCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Bleed);
        }
    }
}