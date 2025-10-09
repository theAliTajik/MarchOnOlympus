using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "MakeshiftDefence_PLUS", menuName = "Cards/MakeshiftDefence_PLUSCard")]
public class MakeshiftDefence_PLUSCard : BaseCardData
{
    public int Fortified;
    
    protected override Type GetActionType()
    {
        return typeof(MakeshiftDefence_PLUSCardAction);
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