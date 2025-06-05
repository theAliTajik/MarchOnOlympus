using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Preparation", menuName = "Cards/PreparationCard")]
public class PreparationCard : BaseCardData
{
    public int DiscardCard;
    public int DrawCard;
    public int StanceDrawCard;
    
    protected override Type GetActionType()
    {
        return typeof(PreparationCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceDrawCard);
        }
        else
        {
            return string.Format(normalDataSet.description, DiscardCard, DrawCard);
        }
    }
}