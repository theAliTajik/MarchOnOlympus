using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Patience", menuName = "Cards/PatienceCard")]
public class PatienceCard : BaseCardData
{
    public int DamagePerNotUsed;
    public int BlockPerNotUsed;
    
    protected override Type GetActionType()
    {
        return typeof(PatienceCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, BlockPerNotUsed);
        }
        else
        {
            return string.Format(normalDataSet.description, DamagePerNotUsed);
        }
    }
}