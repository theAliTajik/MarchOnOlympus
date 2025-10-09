using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "MakeshiftDefence_STAR", menuName = "Cards/MakeshiftDefence_STARCard")]
public class MakeshiftDefence_STARCard : BaseCardData
{
    public int Frenzy;
    
    protected override Type GetActionType()
    {
        return typeof(MakeshiftDefence_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Frenzy);
        }
    }
}