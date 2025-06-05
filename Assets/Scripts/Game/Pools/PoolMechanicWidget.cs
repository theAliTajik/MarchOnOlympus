using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMechanicWidget : Singleton<PoolMechanicWidget>
{
    [SerializeField] private MechanicWidget m_prefab;
    [SerializeField] private int m_defaultCount;


    private Pool<MechanicWidget> m_pool;


    protected override void Init()
    {
        m_pool = new Pool<MechanicWidget>(true, m_prefab, m_defaultCount, transform);
    }

    public MechanicWidget GetItem()
    {
        return m_pool.GetElement();
    }

    public void ReturnToPool(MechanicWidget widget)
    {
        m_pool.ReturnElement(widget);
    }
}