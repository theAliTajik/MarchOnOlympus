using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SerpentPoison", menuName = "Cards/SerpentPoisonCard")]
public class SerpentPoisonCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(SerpentPoisonCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return normalDataSet.description;
        }
    }
}