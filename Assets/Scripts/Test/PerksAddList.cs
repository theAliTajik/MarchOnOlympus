using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerksAddList : MonoBehaviour
{
    [SerializeField] private ClickableList m_list;
    [SerializeField] private PerksDisplay perksDisplay;

    private bool m_isInitialized;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (m_isInitialized)
        {
            return;
        }

        foreach (PerksDb.PerksInfo perk in PerksDb.Instance.AllPerks)
        {
            if (perk.IsImplemented)
            {
                SelectableItemDisplayData m = new SelectableItemDisplayData()
                {
                    Id = perk.ClientID,
                    Text = perk.ClientID,
                    Sprite = perk.Icon
                };
                m_list.AddItem(m);
            }
        }

        m_list.ItemClicked += ItemClicked;

        m_isInitialized = true;
    }

    private void ItemClicked(string clientId)
    {
        PerksManager.Instance.AddPerk(clientId);
        m_list.gameObject.SetActive(false);
    }

    private void ButtonClicked()
    {
        m_list.gameObject.SetActive(true);
    }
}
