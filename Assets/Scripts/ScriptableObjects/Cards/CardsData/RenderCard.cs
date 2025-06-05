using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Render", menuName = "Cards/Render Card")]
public class RenderCard : BaseCardData
{
    
    
    protected override Type GetActionType()
    {
        return typeof(RenderCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return normalDataSet.description;
        }
    }
}