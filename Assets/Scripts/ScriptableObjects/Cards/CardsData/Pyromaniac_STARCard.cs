using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Pyromaniac_STAR", menuName = "Cards/Pyromaniac_STARCard")]
public class Pyromaniac_STARCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(Pyromaniac_STARCardAction);
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