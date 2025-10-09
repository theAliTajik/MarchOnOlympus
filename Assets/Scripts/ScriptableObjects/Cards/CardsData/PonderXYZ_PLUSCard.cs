using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "PonderXYZ_PLUS", menuName = "Cards/PonderXYZ_PLUSCard")]
public class PonderXYZ_PLUSCard : BaseCardData
{
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(PonderXYZ_PLUSCardAction);
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