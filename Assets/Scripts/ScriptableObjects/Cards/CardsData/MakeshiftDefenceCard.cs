using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "MakeshiftDefence", menuName = "Cards/MakeshiftDefenceCard")]
public class MakeshiftDefenceCard : BaseCardData
{
    public int Fortified;
    
    protected override Type GetActionType()
    {
        return typeof(MakeshiftDefenceCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Fortified);
        }
    }
}