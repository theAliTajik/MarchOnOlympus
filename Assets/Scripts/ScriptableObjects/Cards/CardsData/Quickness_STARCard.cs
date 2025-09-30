using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Quickness_STAR", menuName = "Cards/Quickness_STARCard")]
public class Quickness_STARCard : BaseCardData
{
    public int Fortified;
    
    protected override Type GetActionType()
    {
        return typeof(Quickness_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Fortified);
        }
    }
}