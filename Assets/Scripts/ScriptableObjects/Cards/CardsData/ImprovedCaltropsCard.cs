using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ImprovedCaltrops", menuName = "Cards/ImprovedCaltropsCard")]
public class ImprovedCaltropsCard : BaseCardData
{
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(ImprovedCaltropsCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Bleed);
        }
    }
}