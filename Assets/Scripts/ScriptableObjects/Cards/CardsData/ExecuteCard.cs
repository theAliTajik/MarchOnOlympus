using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Execute", menuName = "Cards/ExecuteCard")]
public class ExecuteCard : BaseCardData
{
    public int Damage;
    public int AdditionalDamage;
    public int HPPrecent;
    public int StanceDamage;
    public int StanceAdditionalDamage;
    public int StanceHPPrecent;
    
    protected override Type GetActionType()
    {
        return typeof(ExecuteCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceDamage, StanceAdditionalDamage, StanceHPPrecent);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, AdditionalDamage, HPPrecent);
        }
    }
}