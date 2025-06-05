using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Apprentice", menuName = "Cards/Apprentice Card")]
public class ApprenticeCard : BaseCardData
{
    public int Damage;
    public string TransformCardName;
    
    protected override Type GetActionType()
    {
        return typeof(ApprenticeCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, TransformCardName);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}