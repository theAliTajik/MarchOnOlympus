using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Invent_STAR", menuName = "Cards/Invent_STARCard")]
public class Invent_STARCard : BaseCardData
{
    public int InventGain;
    public int Burn;
    
    protected override Type GetActionType()
    {
        return typeof(Invent_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, InventGain, Burn);
        }
    }
}