using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PonderXYZ_STAR", menuName = "Cards/PonderXYZ_STARCard")]
public class PonderXYZ_STARCard : BaseCardData
{
    public int Invent;
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(PonderXYZ_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Invent, Damage);
        }
    }
}