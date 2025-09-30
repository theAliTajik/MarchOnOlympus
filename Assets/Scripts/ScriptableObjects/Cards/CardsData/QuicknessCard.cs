using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Quickness", menuName = "Cards/QuicknessCard")]
public class QuicknessCard : BaseCardData
{
    public int Block;
    
    protected override Type GetActionType()
    {
        return typeof(QuicknessCardAction);
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