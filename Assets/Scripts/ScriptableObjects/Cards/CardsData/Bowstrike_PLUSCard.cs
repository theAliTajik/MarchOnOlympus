using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Bowstrike_PLUS", menuName = "Cards/Bowstrike_PLUSCard")]
public class Bowstrike_PLUSCard : BaseCardData
{
    public int Daze;
    public int InventLevelThreshold;
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(Bowstrike_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Daze, InventLevelThreshold, Damage);
        }
    }
}