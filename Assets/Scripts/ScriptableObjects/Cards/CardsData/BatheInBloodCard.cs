using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Bathe In Blood", menuName = "Cards/Bathe In Blood Card")]
public class BatheInBloodCard : BaseCardData
{
    
    
    protected override Type GetActionType()
    {
        return typeof(BatheInBloodCardAction);
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