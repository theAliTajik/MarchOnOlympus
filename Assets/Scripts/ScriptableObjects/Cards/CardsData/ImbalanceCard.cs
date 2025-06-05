using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Imbalance", menuName = "Cards/ImbalanceCard")]
public class ImbalanceCard : BaseCardData
{
    public int Damage;
    public int StanceDamage;
    
    protected override Type GetActionType()
    {
        return typeof(ImbalanceCardAction);
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