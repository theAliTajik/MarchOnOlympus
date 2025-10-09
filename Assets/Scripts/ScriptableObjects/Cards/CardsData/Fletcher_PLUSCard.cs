using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Fletcher_PLUS", menuName = "Cards/Fletcher_PLUSCard")]
public class Fletcher_PLUSCard : BaseCardData
{
    public int NumOfArrowsToSpawn;
    
    protected override Type GetActionType()
    {
        return typeof(Fletcher_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, NumOfArrowsToSpawn);
        }
    }
}