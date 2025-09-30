using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Overplan_PLUS", menuName = "Cards/Overplan_PLUSCard")]
public class Overplan_PLUSCard : BaseCardData
{
    public int Vulnerable;
    public int Fortified;
    
    protected override Type GetActionType()
    {
        return typeof(Overplan_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, Vulnerable);
        }
        else
        {
            return string.Format(normalDataSet.description, Vulnerable, Fortified);
        }
    }
}