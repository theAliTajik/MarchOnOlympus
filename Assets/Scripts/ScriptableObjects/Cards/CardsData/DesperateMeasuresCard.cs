using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "DesperateMeasures", menuName = "Cards/DesperateMeasuresCard")]
public class DesperateMeasuresCard : BaseCardData
{
    [FormerlySerializedAs("CostMultiplierForDamage")] public int CostMultiplierForRestore;
    [FormerlySerializedAs("StanceCostMultiplierForDamage")] public int StanceCostMultiplierForRestore;
    
    protected override Type GetActionType()
    {
        return typeof(DesperateMeasuresCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceCostMultiplierForRestore);
        }
        else
        {
            return string.Format(normalDataSet.description, CostMultiplierForRestore);
        }
    }
}