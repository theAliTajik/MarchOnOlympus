using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "AndThrust", menuName = "Cards/AndThrust")]
public class AndThrustCard : BaseCardData
{
    public int Damage;
    public int Impale;

    public string PreviousCardName;
    public string SpawnCardName;
    
    
    protected override Type GetActionType()
    {
        return typeof(AndThrustCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, PreviousCardName, SpawnCardName);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Impale);
        }
    }
}