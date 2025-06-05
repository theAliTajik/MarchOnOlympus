using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCardClickableItem : Singleton<PoolCardClickableItem>
{
    [SerializeField] private CardClickableItem m_prefab;
    [SerializeField] private int m_defaultCount;


    private Pool<CardClickableItem> m_pool;


    protected override void Init()
    {
        m_pool = new Pool<CardClickableItem>(true, m_prefab, m_defaultCount, transform);
    }

    public CardClickableItem GetItem()
    {
        return m_pool.GetElement();
    }

    public void ReturnToPool(CardClickableItem card)
    {
        m_pool.ReturnElement(card);
    }

}
