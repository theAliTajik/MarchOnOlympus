using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PrizeForBlood", menuName = "Cards/PrizeForBloodCard")]
public class PrizeForBloodCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(PrizeForBloodCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, Damage);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}