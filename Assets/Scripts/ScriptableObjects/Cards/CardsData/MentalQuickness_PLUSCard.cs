using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "MentalQuickness_PLUS", menuName = "Cards/MentalQuickness_PLUSCard")]
public class MentalQuickness_PLUSCard : BaseCardData
{
    public int InventDivisor;
    
    protected override Type GetActionType()
    {
        return typeof(MentalQuickness_PLUSCardAction);
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