using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolDeckItem : Singleton<PoolDeckItem>
{
    [SerializeField] private DeckItem m_prefab;
    [SerializeField] private int m_defaultCount;
    
    private Pool<DeckItem> m_pool;


    protected override void Init()
    {
        m_pool = new Pool<DeckItem>(true, m_prefab, m_defaultCount, transform);
    }
    
    public DeckItem GetItem()
    {
        return m_pool.GetElement();
    }

    public void ReturnToPool(DeckItem item)
    {
        m_pool.ReturnElement(item, false);
    }
    
    
}
