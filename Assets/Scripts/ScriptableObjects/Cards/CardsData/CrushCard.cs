using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Crush", menuName = "Cards/CrushCard")]
public class CrushCard : BaseCardData
{
    public int Damage;
    public int CostLess;
    
    protected override Type GetActionType()
    {
        return typeof(CrushCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, CostLess);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}