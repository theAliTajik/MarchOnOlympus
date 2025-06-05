using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Caltrops", menuName = "Cards/CaltropsCard")]
public class CaltropsCard : BaseCardData
{
    public CardType CardType;
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(CaltropsCardAction);
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