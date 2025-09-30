using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Pyromaniac_PLUS", menuName = "Cards/Pyromaniac_PLUSCard")]
public class Pyromaniac_PLUSCard : BaseCardData
{
    public int Restore;
    
    protected override Type GetActionType()
    {
        return typeof(Pyromaniac_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Restore);
        }
    }
}