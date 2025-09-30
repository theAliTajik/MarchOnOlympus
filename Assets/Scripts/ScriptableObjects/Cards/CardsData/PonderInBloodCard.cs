using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PonderInBlood", menuName = "Cards/PonderInBloodCard")]
public class PonderInBloodCard : BaseCardData
{
    
    protected override Type GetActionType()
    {
        return typeof(PonderInBloodCardAction);
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