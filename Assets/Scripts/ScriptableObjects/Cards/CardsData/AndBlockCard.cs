using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "AndBlock", menuName = "Cards/AndBlock Card")]
public class AndBlockCard : BaseCardData
{
    public int Block;
    
    public string PreviousCardName;
    public string SpawnCardName;
    
    protected override Type GetActionType()
    {
        return typeof(AndBlockCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, PreviousCardName, SpawnCardName);
        }
        else
        {
            return string.Format(normalDataSet.description, Block);
        }
    }
}