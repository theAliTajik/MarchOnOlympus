using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableList : ISelector<string>
{
    public event Action<string> ItemClicked;

    [SerializeField] private Transform m_itemsParent;
    [SerializeField] private ClickableItem m_prefab;

    public override void StartSelect(SelectionData data)
    {
        if (data.EfficientMode && m_itemsParent.childCount > 0)
        {
            gameObject.SetActive(true);
            return;
        }
        
        
        gameObject.SetActive(true);
        Clear();
        foreach (var item in data.Enumerable)
        {
            AddItem(item);
        }
    }

    public override void StopSelect()
    {
        Close();
    }

    public virtual void AddItem(SelectableItemDisplayData itemData)
    {
        ClickableItem item = Instantiate(m_prefab, m_itemsParent);
        item.transform.localScale = Vector3.one;
        item.Config(itemData.Id, itemData.Text, itemData.Sprite, OnItemClicked);
    }

    public virtual void Clear()
    {
        foreach (Transform child in m_itemsParent)
        {
            Destroy(child.gameObject);
        }
    }
    
    protected virtual void OnItemClicked(string clientId)
    {
        ItemClicked?.Invoke(clientId);
        RaiseOnSelect(clientId);
    }

    protected virtual void OnEnable()
    {
        m_itemsParent.localScale = Vector3.zero;
        m_itemsParent.DOScale(Vector3.one, 0.25f);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

}