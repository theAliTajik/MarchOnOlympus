using System;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "NoRestForTheWicked", menuName = "Cards/NoRestForTheWickedCard")]
public class NoRestForTheWickedCard : BaseCardData
{
    public Stance CardStanceToCheck;
    public int RestorePerCard;
    
    protected override Type GetActionType()
    {
        return typeof(NoRestForTheWickedCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, CardStanceToCheck.ToString().ToLower(), RestorePerCard);
        }
    }
}