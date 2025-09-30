using System;
using UnityEngine;


[CreateAssetMenu(fileName = "RendingMechanism", menuName = "Cards/RendingMechanismCard")]
public class RendingMechanismCard : BaseCardData
{
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(RendingMechanismCardAction);
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