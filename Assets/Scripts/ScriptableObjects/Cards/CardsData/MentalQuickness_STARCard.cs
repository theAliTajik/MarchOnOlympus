using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "MentalQuickness_STAR", menuName = "Cards/MentalQuickness_STARCard")]
public class MentalQuickness_STARCard : BaseCardData
{
    public int InventDivisor;
    
    protected override Type GetActionType()
    {
        return typeof(MentalQuickness_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, InventDivisor);
        }
    }
}