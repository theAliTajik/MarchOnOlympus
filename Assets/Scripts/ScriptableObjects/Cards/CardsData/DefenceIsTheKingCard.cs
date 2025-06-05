using System;
using UnityEngine;


[CreateAssetMenu(fileName = "DefenceIsTheKing", menuName = "Cards/DefenceIsTheKingCard")]
public class DefenceIsTheKingCard : BaseCardData
{
    public CardPacks cardPack;
    public int Damage;
    public string DescriptionToAddToCards;
    
    protected override Type GetActionType()
    {
        return typeof(DefenceIsTheKingCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage);
        }
    }
}