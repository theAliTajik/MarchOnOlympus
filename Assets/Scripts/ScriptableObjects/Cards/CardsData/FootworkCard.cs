using System;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Footwork", menuName = "Cards/FootworkCard")]
public class FootworkCard : BaseCardData
{
    public int Str;
    public string TransformCardName;
    public Stance SwitchToStance;
    
    protected override Type GetActionType()
    {
        return typeof(FootworkCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, TransformCardName, SwitchToStance.ToString());
        }
        else
        {
            return string.Format(normalDataSet.description, Str);
        }
    }
}