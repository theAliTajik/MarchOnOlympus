using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPerkSeperatorWidget : Singleton<PoolPerkSeperatorWidget>
{
    [SerializeField] private PerksSeperatorWidget m_prefab;
    [SerializeField] private int m_defaultCount;


    private Pool<PerksSeperatorWidget> m_pool;


    protected override void Init()
    {
        m_pool = new Pool<PerksSeperatorWidget>(true, m_prefab, m_defaultCount, transform);
    }

    public PerksSeperatorWidget GetItem()
    {
        return m_pool.GetElement();
    }

    public void ReturnToPool(PerksSeperatorWidget widget)
    {
        m_pool.ReturnElement(widget);
    }
}