using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Offload", menuName = "Cards/OffloadCard")]
public class OffloadCard : BaseCardData
{
    public int Discard;
    public int Improvise;
    
    protected override Type GetActionType()
    {
        return typeof(OffloadCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Discard, Improvise);
        }
    }
}