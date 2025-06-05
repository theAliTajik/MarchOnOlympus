using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCardDisplay : Singleton<PoolCardDisplay> 
{
    [SerializeField] private CardDisplay m_prefab;
    [SerializeField] private int m_defaultCount;
    
    private Pool<CardDisplay> m_pool;


    protected override void Init()
    {
        m_pool = new Pool<CardDisplay>(true, m_prefab, m_defaultCount, transform);
    }

    public CardDisplay GetItem()
    {
        return m_pool.GetElement();
    }

    public void ReturnToPool(CardDisplay card)
    {
        m_pool.ReturnElement(card);
    }
    
    
}
