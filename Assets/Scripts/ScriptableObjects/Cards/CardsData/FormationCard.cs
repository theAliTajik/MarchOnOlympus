using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Formation", menuName = "Cards/FormationCard")]
public class FormationCard : BaseCardData
{
    public int NumOfRandCards;
    public int DamageBuff;
    
    public int StanceNumOfRandCards;

    public string DescriptionToAppendToCards;
    
    protected override Type GetActionType()
    {
        return typeof(FormationCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return string.Format(stanceDataSet.description, StanceNumOfRandCards);
        }
        else
        {
            return string.Format(normalDataSet.description, NumOfRandCards, DamageBuff);
        }
    }
}