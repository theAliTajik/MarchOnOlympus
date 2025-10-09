using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Rejuvenate", menuName = "Cards/RejuvenateCard")]
public class RejuvenateCard : BaseCardData
{
    public int Restore;
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(RejuvenateCardAction);
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