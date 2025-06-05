using System;
using System.Text.RegularExpressions;
using UnityEngine;


[CreateAssetMenu(fileName = "Ice", menuName = "Cards/IceCard")]
public class IceCard : BaseCardData
{
    public int Dex;
    public string TransformCardName;
    public int Restore;
    
    protected override Type GetActionType()
    {
        return typeof(IceCardAction);
    }
    
    public override string GetDescription(bool isInStance)
    {
        if (isInStance)
        {
            string TransformCardNameSeperatedByCapitals = Regex.Replace(TransformCardName, "(?<!^)([A-Z])", " $1");
            return string.Format(stanceDataSet.description, TransformCardNameSeperatedByCapitals, Restore);
        }
        else
        {
            return string.Format(normalDataSet.description, Dex);
        }
    }
}