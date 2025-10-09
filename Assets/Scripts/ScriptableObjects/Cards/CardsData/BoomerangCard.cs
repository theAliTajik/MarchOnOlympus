using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Boomerang", menuName = "Cards/BoomerangCard")]
public class BoomerangCard : BaseCardData
{
    public int Damage;
    public int ExtraDamageEachTimeDiscarded;
    
    protected override Type GetActionType()
    {
        return typeof(BoomerangCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, ExtraDamageEachTimeDiscarded);
        }
    }
}