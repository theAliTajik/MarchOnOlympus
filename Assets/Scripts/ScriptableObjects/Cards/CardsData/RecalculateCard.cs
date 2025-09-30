using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Recalculate", menuName = "Cards/RecalculateCard")]
public class RecalculateCard : BaseCardData
{
    public int Discard;
    public int Draw;
    
    protected override Type GetActionType()
    {
        return typeof(RecalculateCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Discard, Draw);
        }
    }
}