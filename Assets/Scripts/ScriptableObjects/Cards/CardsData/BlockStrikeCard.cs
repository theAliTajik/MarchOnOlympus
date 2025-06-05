using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Strike Card", menuName = "Cards/Block Strike Card")]
public class BlockStrikeCard : BaseCardData
{
    public int Block;
    public int Damage;

    protected override Type GetActionType()
    {
        return typeof(BlockStrikeCardAction);
    }
    
    public override string GetDescription(bool Stance)
    {
        if (Stance)
        {
            return string.Format(stanceDataSet.description, Damage);
        }
        else
        {
            return string.Format(normalDataSet.description, Block);
        }
    }
}
