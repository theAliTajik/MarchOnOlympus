using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Amok", menuName = "Cards/AmokCard")]
public class AmokCard : BaseCardData
{
    public int[] Damages;
    public int[] StanceDamages;

    public int DamageToSelf;
    
    protected override Type GetActionType()
    {
        return typeof(AmokCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            string arrayToCommaSeparated = string.Join(", ", StanceDamages);
            return string.Format(stanceDataSet.description, arrayToCommaSeparated, DamageToSelf);
        }
        else
        {
            string arrayToCommaSeparated = string.Join(", ", Damages);
            return string.Format(normalDataSet.description, arrayToCommaSeparated);
        }
    }
}