using System;
using UnityEngine;


[CreateAssetMenu(fileName = "DaggerMaster", menuName = "Cards/DaggerMasterCard")]
public class DaggerMasterCard : BaseCardData
{
    public int DaggerRemoveAmount;
    public int DaggerCardSpawnAmount;
    public string DaggerCardID;
    
    protected override Type GetActionType()
    {
        return typeof(DaggerMasterCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, DaggerRemoveAmount);
        }
    }
}