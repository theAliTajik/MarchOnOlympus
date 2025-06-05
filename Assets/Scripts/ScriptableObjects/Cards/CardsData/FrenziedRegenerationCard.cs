using System;
using UnityEngine;


[CreateAssetMenu(fileName = "FrenziedRegeneration", menuName = "Cards/FrenziedRegenerationCard")]
public class FrenziedRegenerationCard : BaseCardData
{
    public int Restore;
    public int MinimumDamageToEnemies;
    public int RestoreIfMinimumMet;
    
    protected override Type GetActionType()
    {
        return typeof(FrenziedRegenerationCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, MinimumDamageToEnemies, RestoreIfMinimumMet);
        }
        else
        {
            return string.Format(normalDataSet.description, Restore);
        }
    }
}