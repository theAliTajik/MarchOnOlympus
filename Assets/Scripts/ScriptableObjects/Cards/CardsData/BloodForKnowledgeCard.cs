using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BloodForKnowledge", menuName = "Cards/BloodForKnowledgeCard")]
public class BloodForKnowledgeCard : BaseCardData
{
    public int Damage;
    public int InventGain;
    
    protected override Type GetActionType()
    {
        return typeof(BloodForKnowledgeCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, InventGain);
        }
    }
}