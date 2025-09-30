using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Boom_PLUS", menuName = "Cards/Boom_PLUSCard")]
public class Boom_PLUSCard : BaseCardData
{
    public int Damage;
    public int Burn;
    
    protected override Type GetActionType()
    {
        return typeof(Boom_PLUSCardAction);
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