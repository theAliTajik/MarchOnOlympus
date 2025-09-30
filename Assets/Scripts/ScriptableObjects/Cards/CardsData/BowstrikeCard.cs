using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Bowstrike", menuName = "Cards/BowstrikeCard")]
public class BowstrikeCard : BaseCardData
{
    public int InventThreshold;
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(BowstrikeCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, InventThreshold, Damage);
        }
    }
}