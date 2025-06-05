using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Whetstone", menuName = "Cards/WhetstoneCard")]
public class WhetstoneCard : BaseCardData
{
    public int ImpaleGain;
    public string AddedDescriptionToCard;
    
    protected override Type GetActionType()
    {
        return typeof(WhetstoneCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ImpaleGain);
        }
    }
}