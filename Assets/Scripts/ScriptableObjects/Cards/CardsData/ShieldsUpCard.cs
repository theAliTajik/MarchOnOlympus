using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ShieldsUp", menuName = "Cards/ShieldsUpCard")]
public class ShieldsUpCard : BaseCardData
{
    public int AdditionalFortifiy;
    public int Daze;
    
    protected override Type GetActionType()
    {
        return typeof(ShieldsUpCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, AdditionalFortifiy);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}