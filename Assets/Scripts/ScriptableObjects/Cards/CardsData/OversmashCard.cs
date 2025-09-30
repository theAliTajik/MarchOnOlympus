using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Oversmash", menuName = "Cards/OversmashCard")]
public class OversmashCard : BaseCardData
{
    public int Damage;
    public int InventGainForEachDebuff;
    
    protected override Type GetActionType()
    {
        return typeof(OversmashCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, InventGainForEachDebuff);
        }
    }
}