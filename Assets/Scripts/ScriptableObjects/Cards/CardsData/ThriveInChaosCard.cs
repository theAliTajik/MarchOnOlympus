using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ThriveInChaos", menuName = "Cards/ThriveInChaosCard")]
public class ThriveInChaosCard : BaseCardData
{
    public int DamageMultiplier;
    
    protected override Type GetActionType()
    {
        return typeof(ThriveInChaosCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, DamageMultiplier);
        }
    }
}