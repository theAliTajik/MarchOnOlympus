using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Study", menuName = "Cards/Study Card")]
public class StudyCard : BaseCardData
{
    
    
    protected override Type GetActionType()
    {
        return typeof(StudyCardAction);
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