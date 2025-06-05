using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Finale", menuName = "Cards/FinaleCard")]
public class FinaleCard : BaseCardData
{
    public int Damage;
    public int DamageDoneThreshold;
    
    protected override Type GetActionType()
    {
        return typeof(FinaleCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, DamageDoneThreshold, Damage);
        }
    }
}