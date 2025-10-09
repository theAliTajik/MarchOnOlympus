using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ImprovisedBomb", menuName = "Cards/ImprovisedBombCard")]
public class ImprovisedBombCard : BaseCardData
{
    public int Damage;
    public int Invent;
    public int Impale;
    
    protected override Type GetActionType()
    {
        return typeof(ImprovisedBombCardAction);
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