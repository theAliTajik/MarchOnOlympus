using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Geometry", menuName = "Cards/GeometryCard")]
public class GeometryCard : BaseCardData
{
    public int InventGain;
    
    protected override Type GetActionType()
    {
        return typeof(GeometryCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, InventGain);
        }
    }
}