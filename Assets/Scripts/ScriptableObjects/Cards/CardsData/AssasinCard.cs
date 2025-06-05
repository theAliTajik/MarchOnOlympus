using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Assasin", menuName = "Cards/Assasin Card")]
public class AssasinCard : BaseCardData
{
    public int Damage;
    
    protected override Type GetActionType()
    {
        return typeof(AssasinCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}