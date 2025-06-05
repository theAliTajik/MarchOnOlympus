using System;
using UnityEngine;


[CreateAssetMenu(fileName = "FortuneFavorsTheBold", menuName = "Cards/FortuneFavorsTheBoldCard")]
public class FortuneFavorsTheBoldCard : BaseCardData
{
    protected override Type GetActionType()
    {
        return typeof(FortuneFavorsTheBoldCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            return stanceDataSet.description;
        }
        else
        {
            return normalDataSet.description;
        }
    }
}