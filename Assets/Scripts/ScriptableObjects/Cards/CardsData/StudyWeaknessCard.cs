using System;
using UnityEngine;


[CreateAssetMenu(fileName = "StudyWeakness", menuName = "Cards/StudyWeaknessCard")]
public class StudyWeaknessCard : BaseCardData
{
    public int Frenzy;
    public int Invent;
    
    protected override Type GetActionType()
    {
        return typeof(StudyWeaknessCardAction);
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