using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Pyromaniac", menuName = "Cards/PyromaniacCard")]
public class PyromaniacCard : BaseCardData
{
    public int Restore;
    
    protected override Type GetActionType()
    {
        return typeof(PyromaniacCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Restore);
        }
    }
}