using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CreativeChaos_STAR", menuName = "Cards/CreativeChaos_STARCard")]
public class CreativeChaos_STARCard : BaseCardData
{
    public int NumOfCardsToSpawn;
    public int CardsCost;
    
    protected override Type GetActionType()
    {
        return typeof(CreativeChaos_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, NumOfCardsToSpawn, CardsCost);
        }
    }
}