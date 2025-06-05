using System;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Dash", menuName = "Cards/DashCard")]
public class DashCard : BaseCardData
{
    public int DrawAmount;
    public int EnergyGain;
    public Stance SwitchToStance;
    
    protected override Type GetActionType()
    {
        return typeof(DashCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, DrawAmount, EnergyGain, SwitchToStance.ToString());
        }
    }
}