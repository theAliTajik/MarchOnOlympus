using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Block", menuName = "Cards/BlockCard")]
public class BlockCard : BaseCardData
{
    public int Block;
    
    protected override Type GetActionType()
    {
        return typeof(BlockCardAction);
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