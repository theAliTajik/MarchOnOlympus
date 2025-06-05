using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "AllIn", menuName = "Cards/AllInCard")]
public class AllInCard : BaseCardData
{
    public int Damage;
    public Stance Stance;
    public int DamageIfStance;

    public int StanceDamage;
    public Stance StanceExtraStance;
    public int StanceExtraDamageIfStance;
    
    protected override Type GetActionType()
    {
        return typeof(AllInCardAction);
        
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceDamage, StanceExtraStance, StanceExtraDamageIfStance);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, Stance, DamageIfStance);
        }
    }
}