using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Pyre", menuName = "Cards/PyreCard")]
public class PyreCard : BaseCardData
{
    public int SelfBurn;
    public int EnemyBurn;
    
    protected override Type GetActionType()
    {
        return typeof(PyreCardAction);
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