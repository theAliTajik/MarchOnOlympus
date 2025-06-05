using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Obliterate", menuName = "Cards/Obliterate Card")]
public class ObliterateCard : BaseCardData
{
    public int Damage;
    public float DelayBetweenAttacks;

    protected override Type GetActionType()
    {
        return typeof(ObliterateCardAction);
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