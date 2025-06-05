using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Repetition", menuName = "Cards/RepetitionCard")]
public class RepetitionCard : BaseCardData
{
    public CardType cardType;
    public int StrGain;
    
    protected override Type GetActionType()
    {
        return typeof(RepetitionCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, cardType.ToString(), StrGain);
        }
    }
}