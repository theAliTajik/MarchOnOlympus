using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "FireArrow_STAR", menuName = "Cards/FireArrow_STARCard")]
public class FireArrow_STARCard : BaseCardData
{
    public int Burn;
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(FireArrow_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Burn, Invent);
        }
    }
}