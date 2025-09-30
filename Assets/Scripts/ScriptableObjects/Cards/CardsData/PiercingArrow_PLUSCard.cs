using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PiercingArrow_PLUS", menuName = "Cards/PiercingArrow_PLUSCard")]
public class PiercingArrow_PLUSCard : BaseCardData
{
    public int Damage;
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(PiercingArrow_PLUSCardAction);
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