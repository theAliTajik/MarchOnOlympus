using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Pyre_PLUS", menuName = "Cards/Pyre_PLUSCard")]
public class Pyre_PLUSCard : BaseCardData
{
    public int SelfBurn;
    public int EnemyBurn;
    
    protected override Type GetActionType()
    {
        return typeof(Pyre_PLUSCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, SelfBurn, EnemyBurn);
        }
    }
}