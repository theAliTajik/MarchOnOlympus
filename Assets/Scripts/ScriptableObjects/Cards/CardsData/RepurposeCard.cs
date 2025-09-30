using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Repurpose", menuName = "Cards/RepurposeCard")]
public class RepurposeCard : BaseCardData
{
    public int Discard;
    public int Draw;
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(RepurposeCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Discard, Draw, Invent);
        }
    }
}