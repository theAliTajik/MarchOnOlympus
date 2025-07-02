
using System.Collections.Generic;

public class ModifiableCardDataSet
{
    public ModifiableParam<int> EnergyCost = new ModifiableParam<int>();
    public ModifiableParam<string> Description = new ModifiableParam<string>();
    public ModifiableParam<TargetType> TargetingType = new ModifiableParam<TargetType>();
    public List<CardActionType> ActionType = new List<CardActionType>();
    public ModifiableParam<bool> DoesPerish = new ModifiableParam<bool>();
    public ModifiableParam<bool> DoesPerishIfNotUsed = new ModifiableParam<bool>();
    
    public ModifiableCardDataSet()
    {
        
    }

    public ModifiableCardDataSet(BaseCardDataSet baseCardDataSet, string desc)
    {
        EnergyCost = baseCardDataSet.energyCost;
        Description = desc;
        TargetingType = baseCardDataSet.targetingType;
        ActionType.AddRange(baseCardDataSet.actionType);
        DoesPerish = baseCardDataSet.doesPerish;
        DoesPerishIfNotUsed = baseCardDataSet.doesPerishIfNotUsed;
    }

    public ModifiableCardDataSet Clone()
    {
        ModifiableCardDataSet clone = new ModifiableCardDataSet();
        clone.EnergyCost = EnergyCost.Clone();
        clone.Description = Description.Clone();
        clone.TargetingType = TargetingType.Clone();
        clone.ActionType.AddRange(ActionType);
        clone.DoesPerish = DoesPerish.Clone();
        clone.DoesPerishIfNotUsed = DoesPerishIfNotUsed.Clone();
        return clone;
    }
}
