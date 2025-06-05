using System;
using System.Collections;
using System.Collections.Generic;
using Game.ModifiableParam;
using Newtonsoft.Json;
using UnityEngine;

public class CardInDeckStanceState : CardInDeckBaseState
{
    // public override event Action OnDataChanged;
    // [JsonProperty] private CardInDeckStateMachine m_stateMachine;
    // [JsonIgnore] private BaseCardData m_cardData;
    [JsonProperty] private BaseCardDataSet m_cardDataSet;
    
    [JsonProperty] private ModifiableCardDataSet m_modifiableCardDataSet;


    public override event Action OnDataChanged;

    public override void Configure(CardInDeckStateMachine cardInDeck)
    {
        // m_stateMachine = cardInDeck;
        // m_cardData = cardInDeck.GetCardData();
        m_cardDataSet = cardInDeck.GetCardData().normalDataSet;
        m_modifiableCardDataSet = new ModifiableCardDataSet(m_cardDataSet);
    }

    public override ModifiableCardDataSet CloneData()
    {
        return m_modifiableCardDataSet.Clone();
    }

    public override void SetData(ModifiableCardDataSet data)
    {
        m_modifiableCardDataSet = data;
    }

    public override void OnEnterState(CardInDeckStateMachine cardInDeck)
    {

    }

    public override void OnExitState()
    {
        
    }

    public override int GetEnergy()
    {
        return m_modifiableCardDataSet.EnergyCost;
    }

    
    public override void SetEnergyOverride(IParamModifier<int> modifier)
    {
        m_modifiableCardDataSet.EnergyCost.AddModifier(modifier);
        OnDataChanged?.Invoke();
    }

    public override void RemoveEnergyOverride(IParamModifier<int> modifier)
    {
        m_modifiableCardDataSet.EnergyCost.RemoveModifier(modifier);
        OnDataChanged?.Invoke();
    }

    public override void RemoveAllEnergyOverrides()
    {
        m_modifiableCardDataSet.EnergyCost.RemoveAllModifiers();
    }

    public override TargetType GetTargetingType()
    {
        return m_modifiableCardDataSet.TargetingType;
    }


    public override void SetDescriptionOverride(string newDescription, bool additive = false)
    {
        IParamModifier<string> modifier;
        if (additive)
        {
            modifier = new AddValueModifier<string>(" || " + newDescription);
        }
        else
        {
            modifier = new SetValueModifier<string>(newDescription);
        }

        // Debug.Log("card name: "+ m_cardData.Name + "| new desc: "  + m_descriptionOverride);
        ModifyDescription(modifier);
        OnDataChanged?.Invoke();
    }

    public void ModifyDescription(IParamModifier<string> modifier)
    {
        m_modifiableCardDataSet.Description.AddModifier(modifier);
    }

    public override void RemoveDescriptionOverride()
    {
        m_modifiableCardDataSet.Description.RemoveAllModifiers();
        OnDataChanged?.Invoke();
    }

    public override string GetDescription()
    {
        return m_modifiableCardDataSet.Description;
    }

    public override List<CardActionType> GetActionsTypes()
    {
        return m_modifiableCardDataSet.ActionType;
    }

    public override bool DoesPerish()
    {
        return m_modifiableCardDataSet.DoesPerish;
    }

    public override bool DoesPerishIfNotUsed()
    {
        return m_modifiableCardDataSet.DoesPerishIfNotUsed;
    }

    public override void SetDoesPerishOverride(bool doesPerish)
    {
        IParamModifier<bool> modifier = new SetValueModifier<bool>(doesPerish);
        m_modifiableCardDataSet.DoesPerish.AddModifier(modifier);
    }

    public override void RemoveDoesPerishOverride()
    {
        m_modifiableCardDataSet.DoesPerish.RemoveAllModifiers();
    }

    public override void SetDoesPerishIfNotUsedOverride(bool doesPerish)
    {
        IParamModifier<bool> modifier = new SetValueModifier<bool>(doesPerish);
        m_modifiableCardDataSet.DoesPerishIfNotUsed.AddModifier(modifier);
    }

    public override void RemoveDoesPerishIfNotUsedOverride()
    {
        m_modifiableCardDataSet.DoesPerishIfNotUsed.RemoveAllModifiers();
    }

    public override void SetTargetTypeOverride(TargetType newTargetType)
    {
        IParamModifier<TargetType>  modifier = new SetValueModifier<TargetType>(newTargetType);
        m_modifiableCardDataSet.TargetingType.AddModifier(modifier);
    }

    public override void RemoveTargetTypeOverride()
    {
        m_modifiableCardDataSet.TargetingType.RemoveAllModifiers();
    }
}
