using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Boom_STAR", menuName = "Cards/Boom_STARCard")]
public class Boom_STARCard : BaseCardData
{
    public int Damage;
    public int Burn;
    
    protected override Type GetActionType()
    {
        return typeof(Boom_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Burn);
        }
    }
}