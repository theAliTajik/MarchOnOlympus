using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Expert", menuName = "Cards/Expert Card")]
public class ExpertCard : BaseCardData
{
    public int Damage;
    public string TranformCardName;
    
    protected override Type GetActionType()
    {
        return typeof(ExpertCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, TranformCardName);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}