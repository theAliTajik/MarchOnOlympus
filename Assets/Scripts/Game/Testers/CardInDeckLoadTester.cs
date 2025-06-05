using System;
using System.Collections;
using System.Collections.Generic;
using Game.ModifiableParam;
using UnityEngine;

public class CardInDeckLoadTester : MonoBehaviour
{
    [SerializeField] private BaseCardData m_testCardData;

    private void Start()
    {
        CardInDeckStateMachine card = new CardInDeckStateMachine();
        card.Configure(m_testCardData);
        LogCard(card);

        IParamModifier<int> energySet = new SetValueModifier<int>(25);
        card.SetEnergyOverride(energySet);
        Debug.Log($"normal state energy: {card.CurrentState.GetEnergy()}");
        SaveCard(card);

        card = LoadCard();
        
        LogCard(card);
        Debug.Log($"normal state energy: {card.CurrentState.GetEnergy()}");
    }


    private void LogCard(CardInDeckStateMachine card)
    {
        Debug.Log($"card data name: {card.GetCardName()}");
    }

    private string m_path = "/Test/modifyTest.text";

    private bool isConfiged = false;
    private void ConfigPath()
    {
        if (isConfiged)
        {
            return;
        }
        
        m_path = Application.persistentDataPath + m_path;
        isConfiged = true;
    }
    
    private void SaveCard(CardInDeckStateMachine card)
    {
        ConfigPath();
        JsonHelper.SaveAdvanced(card, m_path);
    }

    private CardInDeckStateMachine LoadCard()
    {
        ConfigPath();
        CardInDeckStateMachine card = JsonHelper.LoadAdvanced<CardInDeckStateMachine>(m_path);
        return card;
    }

}
