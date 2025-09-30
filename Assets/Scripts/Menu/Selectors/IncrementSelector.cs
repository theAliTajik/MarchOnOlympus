
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IncrementSelector : ISelector<string>
{
    private List<SelectableItemDisplayData> m_items;
    private SelectionData m_data;
    
    public override void StartSelect(SelectionData data)
    {
        m_data = data;
        m_items ??= data.Enumerable.ToList();
        if (!data.EfficientMode)
        {
            m_items = data.Enumerable.ToList();
        }
        
    }

    public override void UpdateData(SelectionData data)
    {
        m_data.CurrentlySelected = data.CurrentlySelected;
    }

    public void IncrementButtonClicked(int direction)
    {
        IterateEnemy(direction);
    }

    private void IterateEnemy(int dir)
    {
        if (m_items == null || m_items.Count <= 0)
        {
            Debug.Log("ERROR: No items to iterate");
            return;
        }

        int currentEnemyIndex = FindCurrentEnemyIndex();

        if (currentEnemyIndex == -1)
        {
            SetItem(m_items.FirstOrDefault().Id);
            return;
        }

        int nextEnemyIndex = CalculateNextEnemyIndex(currentEnemyIndex, dir);
        if (nextEnemyIndex == -1) return;
        
        SetItem(m_items[nextEnemyIndex].Id);
    }

    private int FindCurrentEnemyIndex()
    {
        int currentEnemyIndex = m_items.FindIndex(x => x.Id == m_data.CurrentlySelected.Id);
        if (currentEnemyIndex == -1)
        {
            Debug.Log("WARNING: Did not find currently selected item");
            return -1;
        }

        Debug.Log($"current index: {currentEnemyIndex}");
        return currentEnemyIndex;
    }

    private int CalculateNextEnemyIndex(int currentEnemyIndex, int dir)
    {
        int nextEnemyIndex = currentEnemyIndex + dir;
        if (nextEnemyIndex == -1)
        {
            nextEnemyIndex = m_items.Count - 1;
        }

        if (nextEnemyIndex >= m_items.Count)
        {
            nextEnemyIndex = 0;
        }
        
        if (nextEnemyIndex < 0 && nextEnemyIndex >= m_items.Count)
        {
            Debug.Log("WARNING: Invalid index inside of iteration on items selection");
            return -1;
        }

        Debug.Log($"next index: {nextEnemyIndex}");
        return nextEnemyIndex;
    }

    private void SetItem(string id)
    {
        Debug.Log($"increment set item: {id}");
        RaiseOnSelect(id);
    }
    
    public override void StopSelect()
    {
    }
}
