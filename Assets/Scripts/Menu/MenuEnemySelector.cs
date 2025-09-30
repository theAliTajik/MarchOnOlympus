
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuEnemySelector : MonoBehaviour
{
    private const string ENEMY_SELECTED_PLAYER_PREFS_KEY = "enemySelected";

    [SerializeField] private ISelector<string> m_selector;
    [SerializeField] private TMP_Text m_enemyText;

    private bool m_isInitialized;
    
    private List<SelectableItemDisplayData> m_enemyItems = new List<SelectableItemDisplayData>();

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

        m_selector = GetComponentInChildren<ISelector<string>>(true);
        if (m_selector == null)
        {
            CustomDebug.LogError("No char selector found", Categories.UI.MainMenu);
            return;
        }

        // CustomDebug.Log("Found selector", Categories.UI.MainMenu);

        m_enemyItems = EnemiesDb.Instance.allEnemies
            .Select(x => new SelectableItemDisplayData
            {
                Id = x.clientID,
                Text = x.clientID,
                Sprite = null
            }).ToList();

        if (m_enemyItems.Count <= 0)
        {
            Debug.Log("ERROR: No enemeis to display");
            return;
        }

        m_selector.OnSelect += ItemClicked;
        m_isInitialized = true;
        
        

        if (!PlayerPrefs.HasKey(ENEMY_SELECTED_PLAYER_PREFS_KEY))
        {
            SetItem(m_enemyItems.FirstOrDefault().Id);
            return;
        }

        string clientId = PlayerPrefs.GetString(ENEMY_SELECTED_PLAYER_PREFS_KEY);
        if (!string.IsNullOrEmpty(clientId))
        {
            SetItem(clientId);
        }

    }

    private void StartSelection()
    {
        SelectionData data = new SelectionData(m_enemyItems, efficientMode: true);
        m_selector.StartSelect(data);
    }

    private void ItemClicked(string clientId)
    {
        m_selector.StopSelect();
        PlayerPrefs.SetString(ENEMY_SELECTED_PLAYER_PREFS_KEY, clientId);
        SetItem(clientId);
    }

    private void SetItem(string clientId)
    {
        m_enemyText.text = clientId;
        GameSessionParams.EnemyClientId = clientId;
    }

    public void ButtonClicked()
    {
        StartSelection();
    }

    public void SwipeButtonClicked(int direction)
    {
        IterateEnemy(direction);
    }

    private void IterateEnemy(int dir)
    {
        if (m_enemyItems.Count <= 0)
        {
            Debug.Log("ERROR: No enemies to iterate");
            return;
        }

        int currentEnemyIndex = FindCurrentEnemyIndex();

        if (currentEnemyIndex == -1)
        {
            SetItem(m_enemyItems.FirstOrDefault().Id);
            return;
        }

        int nextEnemyIndex = CalculateNextEnemyIndex(currentEnemyIndex, dir);
        if (nextEnemyIndex == -1) return;
        
        SetItem(m_enemyItems[nextEnemyIndex].Id);
    }

    private int FindCurrentEnemyIndex()
    {
        int currentEnemyIndex = m_enemyItems.FindIndex(x => x.Id == m_enemyText.text);
        if (currentEnemyIndex == -1)
        {
            Debug.Log("WARNING: Did not find currently selected enemy");
            return -1;
        }

        return currentEnemyIndex;
    }

    private int CalculateNextEnemyIndex(int currentEnemyIndex, int dir)
    {
        int nextEnemyIndex = currentEnemyIndex + dir;
        if (nextEnemyIndex == -1)
        {
            nextEnemyIndex = m_enemyItems.Count - 1;
        }
        
        if (nextEnemyIndex < 0 && nextEnemyIndex >= m_enemyItems.Count)
        {
            Debug.Log("WARNING: Invalid index inside of iteration on enemy selection");
            return -1;
        }
        
        return nextEnemyIndex;
    }
}
