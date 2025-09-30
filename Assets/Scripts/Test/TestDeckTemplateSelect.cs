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

    private List<string> m_decks = new List<string>();
    private bool m_isInitialized;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        MenuEventBus.OnDeckSelected += SetSelected;
        Init();
    }

    public void Init()
    {
        if (m_isInitialized)
            return;

        DeckTemplates.LoadAllDecks();
        LoadAllDecks();

        string selectedDeckId = PlayerPrefs.HasKey(DECK_SELECTED) 
            ? PlayerPrefs.GetString(DECK_SELECTED) 
            : null;

        if (string.IsNullOrEmpty(selectedDeckId) && m_decks.Count > 0)
        {
            selectedDeckId = m_decks[0];
        }

        if (!string.IsNullOrEmpty(selectedDeckId))
        {
            SetSelected(selectedDeckId);
        }

        m_list.ItemClicked += ItemClicked;
        m_isInitialized = true;
    }

    private void LoadAllDecks()
    {
        m_list.Clear();
        foreach (DeckTemplates.Deck deck in DeckTemplates.Decks)
        {
            SelectableItemDisplayData m = new SelectableItemDisplayData()
            {
                Id = deck.clientID,
                Text = deck.clientID,
            };
            m_list.AddItem(m);
            m_decks.Add(deck.clientID);
        }
    }

    private void ItemClicked(string clientId)
    {
        m_list.gameObject.SetActive(false);
        SetSelected(clientId);
    }

    private void SetSelected(string clientId)
    {
        PlayerPrefs.SetString(DECK_SELECTED, clientId);
        m_deckTemplateText.text = clientId;
        GameSessionParams.DeckTemplateClientId = clientId;
    }

    private void ButtonClicked()
    {
        LoadAllDecks();
        m_list.gameObject.SetActive(true);
    }
}
