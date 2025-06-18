using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Game;
using Game.ModifiableParam;
using Newtonsoft.Json;
using UnityEngine;

public enum ECardInDeckState
{
    NORMAL,
    STANCE
}

public class CardInDeckStateMachine
{
    [JsonIgnore] public Action OnDataChanged;
    
    // states
    [JsonProperty] private CardInDeckBaseState m_currentState;
    
    [JsonProperty] private CardInDeckNormalState m_normalState = new CardInDeckNormalState();
    [JsonProperty] private CardInDeckStanceState m_stanceState = new CardInDeckStanceState();
    
    
    public CardInDeckBaseState CurrentState { get { return m_currentState; } }
    
    public CardInDeckBaseState NormalState { get { return m_normalState; } }
    public CardInDeckBaseState StanceState { get { return m_stanceState; } }
    
    
    // Data
    [JsonIgnore] private BaseCardData m_cardData;
    [JsonProperty] private string m_cardDataId;
    
    private BaseCardData CardData
    {
        get => m_cardData;
    }
    
    [OnDeserialized]
    private void OnAfterLoad(StreamingContext _)
    {
        m_cardData = CardsDb.Instance.FindById(m_cardDataId)?.CardData;
    }
    
    public void Configure(BaseCardData cardData)
    {
        m_cardData = cardData;
        m_cardDataId = CardsDb.Instance.GetID(cardData);
        m_normalState.Configure(this);
        m_stanceState.Configure(this);
        
        m_currentState = m_normalState;
        CurrentState.OnEnterState(this);
    }

    public CardInDeckStateMachine Clone()
    {
        CardInDeckStateMachine clone = new CardInDeckStateMachine();
        clone.Configure(m_cardData);
        clone.NormalState.SetData(m_normalState.CloneData());
        clone.StanceState.SetData(m_stanceState.CloneData());
        return clone;
    }
    
    public void ChangeState(Stance stance)
    {
        if (stance == m_cardData.MStance)
        {
            ChangeState(m_stanceState);
        }
        else
        {
            ChangeState(m_normalState);
        }
    }
    
    public void ChangeState(CardInDeckBaseState newState)
    {
        if (newState == m_currentState)
        {
            return;
        }
        m_currentState = newState;
        CurrentState.OnEnterState(this);
    }

    public BaseCardAction InstantiateCardAction()
    {
        return m_cardData.InstantiateAction();
    }

    public string GetCardName()
    {
        return m_cardData.Name;
    }

    public CardType GetCardType()
    {
        return m_cardData.Type;
    }

    public Sprite GetCardImage()
    {
        return m_cardData.Image;
    }

    public Stance GetStance()
    {
        return m_cardData.MStance;
    }

    public CardPacks GetCardPack()
    {
        return m_cardData.CardPack;
    }

    public BaseCardData GetCardData()
    {
        return m_cardData;
    }

    public void InvokeOnDataChanged()
    {
        OnDataChanged?.Invoke();
    }

    public void SetEnergyOverride(IParamModifier<int> energyModifier)
    {
        m_normalState.SetEnergyOverride(energyModifier);
        m_stanceState.SetEnergyOverride(energyModifier);
    }

    public void RemoveEnergyOverride()
    {
        m_normalState.RemoveAllEnergyOverrides();
        m_stanceState.RemoveAllEnergyOverrides();
    }
}
