using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Punisher", menuName = "Cards/PunisherCard")]
public class PunisherCard : BaseCardData
{
    public int Damage;
    public int StanceDamage;
    
    protected override Type GetActionType()
    {
        return typeof(PunisherCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceDamage);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}