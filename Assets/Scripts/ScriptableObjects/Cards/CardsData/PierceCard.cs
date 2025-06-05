using System;
using Game;
using UnityEngine;


[CreateAssetMenu(fileName = "Pierce", menuName = "Cards/PierceCard")]
public class PierceCard : BaseCardData
{
    public int Damage;
    public string TransformCardName;
    public Stance SwitchToStance;
    
    protected override Type GetActionType()
    {
        return typeof(PierceCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, TransformCardName, SwitchToStance.ToString());
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}