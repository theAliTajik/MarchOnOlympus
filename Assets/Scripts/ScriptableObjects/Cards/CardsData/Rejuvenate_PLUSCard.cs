using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Rejuvenate_PLUS", menuName = "Cards/Rejuvenate_PLUSCard")]
public class Rejuvenate_PLUSCard : BaseCardData
{
    public int Restore;
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(Rejuvenate_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Restore, Invent);
        }
    }
}