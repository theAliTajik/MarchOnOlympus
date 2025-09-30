using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Oversmart_PLUS", menuName = "Cards/Oversmart_PLUSCard")]
public class Oversmart_PLUSCard : BaseCardData
{
    public int Draw;
    public int Fortified;
    public int Energy;
    

    protected override Type GetActionType()
    {
        return typeof(Oversmart_PLUSCardAction);
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