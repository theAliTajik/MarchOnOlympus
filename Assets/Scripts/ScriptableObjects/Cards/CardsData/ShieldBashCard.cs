using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Shield Bash", menuName = "Cards/Shield Bash Card")]
public class ShieldBashCard : BaseCardData
{
    public int StanceDamage;
    public int Daze;
    
    protected override Type GetActionType()
    {
        return typeof(ShieldBashCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, StanceDamage);
        }
        else
        {
            return normalDataSet.description;
        }
    }
}