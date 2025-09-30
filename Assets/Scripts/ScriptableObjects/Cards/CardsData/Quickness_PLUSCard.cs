using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Quickness_PLUS", menuName = "Cards/Quickness_PLUSCard")]
public class Quickness_PLUSCard : BaseCardData
{
    public int Block;
    
    protected override Type GetActionType()
    {
        return typeof(Quickness_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Block);
        }
    }
}