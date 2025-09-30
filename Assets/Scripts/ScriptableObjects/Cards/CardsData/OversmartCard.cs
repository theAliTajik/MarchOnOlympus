using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Oversmart", menuName = "Cards/OversmartCard")]
public class OversmartCard : BaseCardData
{
    public int Draw;
    public int Fortified;
    public int Energy;
    
    protected override Type GetActionType()
    {
        return typeof(OversmartCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Draw, Fortified, Energy);
        }
    }
}