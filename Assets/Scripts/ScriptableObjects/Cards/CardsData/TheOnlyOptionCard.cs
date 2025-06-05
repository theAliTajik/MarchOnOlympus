using System;
using UnityEngine;


[CreateAssetMenu(fileName = "TheOnlyOption", menuName = "Cards/TheOnlyOptionCard")]
public class TheOnlyOptionCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(TheOnlyOptionCardAction);
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