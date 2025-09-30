using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Overplan_STAR", menuName = "Cards/Overplan_STARCard")]
public class Overplan_STARCard : BaseCardData
{
    public int Discard;
    public int Fortified;
    
    protected override Type GetActionType()
    {
        return typeof(Overplan_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Discard, Fortified);
        }
    }
}