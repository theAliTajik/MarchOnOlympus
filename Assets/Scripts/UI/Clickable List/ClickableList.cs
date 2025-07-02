using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableList : MonoBehaviour
{
    public event Action<string> ItemClicked;


    [SerializeField] private Transform m_itemsParent;
    [SerializeField] private ClickableItem m_prefab;

    public virtual void AddItem(string clientId, string text, Sprite image)
    {
        ClickableItem item = Instantiate(m_prefab, m_itemsParent);
        item.transform.localScale = Vector3.one;
        item.Config(clientId, text, image, OnItemClicked);
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