
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckQuickSelectPresenter : MonoBehaviour
{
    [SerializeField] private List<ISelector<int>> m_selectors;

    private void Awake()
    {
        foreach (var view in m_selectors)
        {
            view.OnSelect += OnDeckSelected;
        }
    }

    private void OnDeckSelected(int i)
    {
        string deckId = ConvertIntToDeckID(i);
        MenuEventBus.SendOnDeckSelected(deckId);
    }

    private const string m_deckPrefix = "DECK_";
    private string ConvertIntToDeckID(int i)
    {
        string id = m_deckPrefix + i.ToString();
        return id;
    }
}
