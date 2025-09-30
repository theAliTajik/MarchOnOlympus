using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Stasis", menuName = "Cards/StasisCard")]
public class StasisCard : BaseCardData
{
    public int BlockPerImprovise;
    
    protected override Type GetActionType()
    {
        return typeof(StasisCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, BlockPerImprovise);
        }
    }
}