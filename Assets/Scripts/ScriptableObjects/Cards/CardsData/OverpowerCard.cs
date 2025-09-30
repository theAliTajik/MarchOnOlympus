using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Overpower", menuName = "Cards/Overpower Card")]
public class OverpowerCard : BaseCardData
{
    public int Damage;
    public int DamageThreshold;
    public int Daze;
    
    protected override Type GetActionType()
    {
        return typeof(OverpowerCardAction);
    }
    
    public override string GetDescription(bool Stance = false)
    {
        Debug.Log($"the overpower stance bool was : {Stance}");
        if (Stance)
        {
            return string.Format(stanceDataSet.description, DamageThreshold);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}