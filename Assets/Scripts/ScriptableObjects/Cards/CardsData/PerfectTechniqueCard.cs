using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PerfectTechnique", menuName = "Cards/PerfectTechniqueCard")]
public class PerfectTechniqueCard : BaseCardData
{
    public int Damage;
    public int BlockGain;
    public int Restore;
    
    protected override Type GetActionType()
    {
        return typeof(PerfectTechniqueCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, Restore);
        }
        else
        {
            return string.Format(normalDataSet.description, Damage, BlockGain);
        }
    }
}