using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ChaosProtects_STAR", menuName = "Cards/ChaosProtects_STARCard")]
public class ChaosProtects_STARCard : BaseCardData
{
    public int ForEeachNumOfCardsInStartingDeck;
    public int DamageButtomRange;
    public int DamageTopRange;
    
    protected override Type GetActionType()
    {
        return typeof(ChaosProtects_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, ForEeachNumOfCardsInStartingDeck, DamageButtomRange, DamageTopRange);
        }
    }
}