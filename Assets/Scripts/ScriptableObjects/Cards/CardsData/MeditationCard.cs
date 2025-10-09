using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Meditation", menuName = "Cards/MeditationCard")]
public class MeditationCard : BaseCardData
{
    protected override Type GetActionType()
    {
        return typeof(MeditationCardAction);
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