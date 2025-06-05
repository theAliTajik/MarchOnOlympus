using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "And Slash", menuName = "Cards/And Slash Card")]
public class AndSlashCard : BaseCardData
{
    public int Damage;
    public int BleedAmount;
    
    
    
    protected override Type GetActionType()
    {
        return typeof(AndSlashCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, BleedAmount);
        }
    }
}