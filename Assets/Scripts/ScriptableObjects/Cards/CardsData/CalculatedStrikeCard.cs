using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CalculatedStrike", menuName = "Cards/CalculatedStrikeCard")]
public class CalculatedStrikeCard : BaseCardData
{
    public int Damage;
    public int Improvize;
    
    protected override Type GetActionType()
    {
        return typeof(CalculatedStrikeCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Improvize);
        }
    }
}