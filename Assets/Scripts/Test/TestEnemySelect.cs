using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestEnemySelect : MonoBehaviour
{
    private const string ENEMY_SELECTED = "enemySelected";

    [SerializeField] private ClickableList m_list;
    [SerializeField] private TMP_Text m_enemyText;

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

        bool firstOne = true;
        foreach (EnemiesDb.EnemyInfo enemy in EnemiesDb.Instance.allEnemies)
        {
            // m_list.AddItem(enemy.clientID, enemy.clientID, null);

            if (firstOne)
            {
                SetItem(enemy.clientID);
                firstOne = false;
            }
        }

        if (PlayerPrefs.HasKey(ENEMY_SELECTED))
        {
            string clientId = PlayerPrefs.GetString(ENEMY_SELECTED);
            if (!string.IsNullOrEmpty(clientId))
            {
                SetItem(clientId);
            }
        }


        m_list.ItemClicked += ItemClicked;

        m_isInitialized = true;
    }

    private void ItemClicked(string clientId)
    {
        m_list.gameObject.SetActive(false);
        PlayerPrefs.SetString(ENEMY_SELECTED, clientId);
        SetItem(clientId);
    }

    private void SetItem(string clientId)
    {
        m_enemyText.text = clientId;
        GameSessionParams.EnemyClientId = clientId;
    }

    private void ButtonClicked()
    {
        m_list.gameObject.SetActive(true);
    }
}
