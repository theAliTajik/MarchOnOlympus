using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ChaosProtects", menuName = "Cards/ChaosProtectsCard")]
public class ChaosProtectsCard : BaseCardData
{
    public int ForeachNumOfCards;
    public int Fortified;
    public int Exile;
    public int Ingenius;
    
    protected override Type GetActionType()
    {
        return typeof(ChaosProtectsCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ForeachNumOfCards, Fortified, Exile, Ingenius);
        }
    }
}