using System;
using UnityEngine;


[CreateAssetMenu(fileName = "WellRoundedWarrior", menuName = "Cards/WellRoundedWarriorCard")]
public class WellRoundedWarriorCard : BaseCardData
{
    public Stance CardStanceToCheck;
    public int NumOfCardsToPlay;
    
    protected override Type GetActionType()
    {
        return typeof(WellRoundedWarriorCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, CardStanceToCheck, NumOfCardsToPlay);
        }
    }
}