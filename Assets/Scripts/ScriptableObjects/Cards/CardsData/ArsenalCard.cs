using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Arsenal", menuName = "Cards/ArsenalCard")]
public class ArsenalCard : BaseCardData
{
    public int ExtraDamage;
    public int Improvise;
    
    protected override Type GetActionType()
    {
        return typeof(ArsenalCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ExtraDamage, Improvise);
        }
    }
}