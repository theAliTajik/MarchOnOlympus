using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "QuickThinking", menuName = "Cards/QuickThinkingCard")]
public class QuickThinkingCard : BaseCardData
{
    public int Draw;
    public int Improvise;
    
    protected override Type GetActionType()
    {
        return typeof(QuickThinkingCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Draw, Improvise);
        }
    }
}