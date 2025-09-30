using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Rejuvenate_STAR", menuName = "Cards/Rejuvenate_STARCard")]
public class Rejuvenate_STARCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(Rejuvenate_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}