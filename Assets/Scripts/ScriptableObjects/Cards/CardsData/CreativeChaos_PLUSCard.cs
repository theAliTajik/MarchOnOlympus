using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "CreativeChaos_PLUS", menuName = "Cards/CreativeChaos_PLUSCard")]
public class CreativeChaos_PLUSCard : BaseCardData
{
    public int ExtraInvent;
    
    protected override Type GetActionType()
    {
        return typeof(CreativeChaos_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ExtraInvent);
        }
    }
}