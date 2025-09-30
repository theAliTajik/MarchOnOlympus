using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MenuVerticalCharacterSelector : ISelector<string>
{
    [SerializeField] private Transform m_ItemsContainer;
    [SerializeField] private ClickableItem m_ItemPrefab;
    
    public override void StartSelect(SelectionData data)
    {
        if (data.EfficientMode && m_ItemsContainer.childCount > 0)
        {
            return;
        }

        foreach (var item in data.Enumerable)
        {
            SpawnItem(item);
        }
    }

    private void SpawnItem(SelectableItemDisplayData data)
    {
        ClickableItem item = Instantiate(m_ItemPrefab, m_ItemsContainer, worldPositionStays: false);
        item.Config(data.Id, data.Text, data.Sprite, RaiseOnSelect);
    }

    public override void StopSelect()
    {
        
    }
}
