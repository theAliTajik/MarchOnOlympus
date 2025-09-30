using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Bowstrike_STAR", menuName = "Cards/Bowstrike_STARCard")]
public class Bowstrike_STARCard : BaseCardData
{
    public int Stun;
    public int InventLevelThreshold;
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(Bowstrike_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Stun, InventLevelThreshold, Damage);
        }
    }
}