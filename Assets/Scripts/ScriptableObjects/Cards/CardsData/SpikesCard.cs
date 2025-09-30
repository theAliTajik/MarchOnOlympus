using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Spikes", menuName = "Cards/SpikesCard")]
public class SpikesCard : BaseCardData
{
    public int Block;
    public int Bleed;
    
    protected override Type GetActionType()
    {
        return typeof(SpikesCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Block, Bleed);
        }
    }
}