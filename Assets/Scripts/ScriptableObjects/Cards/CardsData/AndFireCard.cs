using System;
using UnityEngine;


[CreateAssetMenu(fileName = "AndFire", menuName = "Cards/AndFireCard")]
public class AndFireCard : BaseCardData
{
    public int Str;
    public string TransformCardName;
    
    protected override Type GetActionType()
    {
        return typeof(AndFireCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, TransformCardName);
        }
        else
        {
            return string.Format(normalDataSet.description, Str);
        }
    }
}