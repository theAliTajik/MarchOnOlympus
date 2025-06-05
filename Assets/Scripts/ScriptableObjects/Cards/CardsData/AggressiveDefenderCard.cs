using System;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "AggressiveDefender", menuName = "Cards/AggressiveDefenderCard")]
public class AggressiveDefenderCard : BaseCardData
{
    public int BlockGain;
    public int DextarityGain;
    public Stance StanceToCheck;
    public Stance StanceToChange;
    public string DescriptionToAdd;
    
    protected override Type GetActionType()
    {
        return typeof(AggressiveDefenderCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, DextarityGain);
        }
        else
        {
            return string.Format(normalDataSet.description, StanceToCheck, StanceToChange, BlockGain);
        }
    }
}