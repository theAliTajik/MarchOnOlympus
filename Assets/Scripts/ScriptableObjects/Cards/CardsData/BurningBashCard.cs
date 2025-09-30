using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BurningBash", menuName = "Cards/BurningBashCard")]
public class BurningBashCard : BaseCardData
{
    public int BurnMultiplier;
    
    protected override Type GetActionType()
    {
        return typeof(BurningBashCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description);
        }
    }
}