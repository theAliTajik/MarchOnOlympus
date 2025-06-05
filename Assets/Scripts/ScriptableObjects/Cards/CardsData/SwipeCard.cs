using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Swipe", menuName = "Cards/SwipeCard")]
public class SwipeCard : BaseCardData
{
    public int Damage;
    [FormerlySerializedAs("CardTypeToCount")] public Stance CardStanceToCount;
    public int ReduceDamage;
    
    protected override Type GetActionType()
    {
        return typeof(SwipeCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, ReduceDamage, CardStanceToCount.ToString());
        }
    }
}