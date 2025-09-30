using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ChaosProtects_PLUS", menuName = "Cards/ChaosProtects_PLUSCard")]
public class ChaosProtects_PLUSCard : BaseCardData
{
    public int ForeachNumOfCardsInStartingDeck;
    public int Fortified;
    public int Strength;
    public int Exile;
    
    protected override Type GetActionType()
    {
        return typeof(ChaosProtects_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ForeachNumOfCardsInStartingDeck, Fortified, Strength, Exile);
        }
    }
}