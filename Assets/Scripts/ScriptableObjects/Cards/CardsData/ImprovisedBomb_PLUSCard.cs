using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ImprovisedBomb_PLUS", menuName = "Cards/ImprovisedBomb_PLUSCard")]
public class ImprovisedBomb_PLUSCard : BaseCardData
{
    public int Damage;
    public int Invent;
    public int Impale;
    
    protected override Type GetActionType()
    {
        return typeof(ImprovisedBomb_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Invent, Impale);
        }
    }
}