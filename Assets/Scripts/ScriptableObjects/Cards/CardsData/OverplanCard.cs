using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Overplan", menuName = "Cards/OverplanCard")]
public class OverplanCard : BaseCardData
{
    public int Vulnerable;
    public int InventLevelThreshold;
    public int Fortified;
    
    protected override Type GetActionType()
    {
        return typeof(OverplanCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, InventLevelThreshold);
        }
    }
}