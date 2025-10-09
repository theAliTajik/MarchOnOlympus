using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "PonderXYZ", menuName = "Cards/PonderXYZCard")]
public class PonderXYZCard : BaseCardData
{
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(PonderXYZCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Invent);
        }
    }
}