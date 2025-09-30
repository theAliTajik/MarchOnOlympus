using System;
using UnityEngine;


[CreateAssetMenu(fileName = "MentalQuickness", menuName = "Cards/MentalQuicknessCard")]
public class MentalQuicknessCard : BaseCardData
{
    public int InventDivisor;
    
    protected override Type GetActionType()
    {
        return typeof(MentalQuicknessCardAction);
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