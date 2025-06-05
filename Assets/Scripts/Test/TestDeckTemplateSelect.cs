using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestDeckTemplateSelect : MonoBehaviour
{
    private const string DECK_SELECTED = "deckSelected";


    [SerializeField] private ClickableList m_list;
    [SerializeField] private TMP_Text m_deckTemplateText;

    private bool m_isInitialized;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        Init();
    }


    public void Init()
    {
        if (m_isInitialized)
        {
            return;
        }
        DeckTemplates.LoadAllDecks();
        bool firstOne = true;
        foreach (DeckTemplates.Deck deck in DeckTemplates.Decks)
        {
            m_list.AddItem(deck.clientID, deck.clientID, null);
            if (firstOne)
            {
                SetSelected(deck.clientID);
                firstOne = false;
            }
        }

        if (PlayerPrefs.HasKey(DECK_SELECTED))
        {
            string clientId = PlayerPrefs.GetString(DECK_SELECTED);
            if (!string.IsNullOrEmpty(clientId))
            {
                SetSelected(clientId);
            }
        }

        m_list.ItemClicked += ItemClicked;

        m_isInitialized = true;
    }

    private void ItemClicked(string clientId)
    {
        m_list.gameObject.SetActive(false);
        PlayerPrefs.SetString(DECK_SELECTED, clientId);
        SetSelected(clientId);
    }

    private void SetSelected(string clientId)
    {
        m_deckTemplateText.text = clientId;
        GameSessionParams.deckTemplateClientId = clientId;
    }

    private void ButtonClicked()
    {
        m_list.gameObject.SetActive(true);
    }
}
