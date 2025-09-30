using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Caltrops_PLUS", menuName = "Cards/Caltrops_PLUSCard")]
public class Caltrops_PLUSCard : BaseCardData
{
    public int Block;
    public int Thorns;
    
    protected override Type GetActionType()
    {
        return typeof(Caltrops_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Block, Thorns);
        }
    }
}