using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "StudyWeakness_STAR", menuName = "Cards/StudyWeakness_STARCard")]
public class StudyWeakness_STARCard : BaseCardData
{
    public int Frenzy;
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(StudyWeakness_STARCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return string.Format(normalDataSet.description, Frenzy, Invent);
        }
    }
}