using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Recalculate_PLUS", menuName = "Cards/Recalculate_PLUSCard")]
public class Recalculate_PLUSCard : BaseCardData
{
    public int Discard;
    public int Draw;
    
    protected override Type GetActionType()
    {
        return typeof(Recalculate_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, Discard);
        }
        else
        {
            return string.Format(normalDataSet.description, Discard, Draw);
        }
    }
}