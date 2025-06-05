using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPerkWidget : Singleton<PoolPerkWidget>
{
    [SerializeField] private PerksWidget m_prefab;
    [SerializeField] private int m_defaultCount;


    private Pool<PerksWidget> m_pool;


    protected override void Init()
    {
        m_pool = new Pool<PerksWidget>(true, m_prefab, m_defaultCount, transform);
    }

    public PerksWidget GetItem()
    {
        return m_pool.GetElement();
    }

    public void ReturnToPool(PerksWidget widget)
    {
        m_pool.ReturnElement(widget);
    }
}