using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Loremaster", menuName = "Cards/LoremasterCard")]
public class LoremasterCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(LoremasterCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}