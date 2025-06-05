using System;
using System.Collections.Generic;
using Game.ModifiableParam;
using Unity.VisualScripting;

public abstract class CardInDeckBaseState
{
    public abstract event Action OnDataChanged;
    public abstract void Configure(CardInDeckStateMachine cardInDeck);
    
    public abstract ModifiableCardDataSet CloneData();
    public abstract void SetData(ModifiableCardDataSet data);
    
    public abstract void OnEnterState(CardInDeckStateMachine cardInDeck);
    public abstract void OnExitState();
    
    public abstract int GetEnergy();

    public abstract void SetEnergyOverride(IParamModifier<int> energyModifier);
    public abstract void RemoveEnergyOverride(IParamModifier<int> energyModifier);
    public abstract void RemoveAllEnergyOverrides();

    public abstract TargetType GetTargetingType();
    
    public abstract void SetDescriptionOverride(string newDescription, bool additive = false);
    public abstract void RemoveDescriptionOverride();
    public abstract string GetDescription();
    
    public abstract List<CardActionType> GetActionsTypes();

    public abstract bool DoesPerish();
    public abstract bool DoesPerishIfNotUsed();

    public abstract void SetDoesPerishOverride(bool doesPerish);
    public abstract void RemoveDoesPerishOverride();
    
    public abstract void SetDoesPerishIfNotUsedOverride(bool doesPerish);
    public abstract void RemoveDoesPerishIfNotUsedOverride();
    
    public abstract void SetTargetTypeOverride(TargetType newTargetType);
    public abstract void RemoveTargetTypeOverride();
    
    
}





    