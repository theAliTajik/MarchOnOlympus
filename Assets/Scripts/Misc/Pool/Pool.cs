using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T> where T : Component
{
    private Queue<T> m_elements;
    private HashSet<int> m_hashes;
    private T m_prefab;
    private bool m_dynamicSize;
    private Transform m_defaultParent;

    public Pool(bool dynamicSize, T prefab, int initialSize, Transform defaultParent)
    {
        m_elements = new Queue<T>();
        m_hashes = new HashSet<int>();

        m_dynamicSize = dynamicSize;
        m_prefab = prefab;
        m_defaultParent = defaultParent;

        for (int j = 0; j < initialSize; ++j)
        {
            T obj = (T)Object.Instantiate(m_prefab);
            obj.transform.name = m_prefab.transform.name;

            obj.transform.SetParent(m_defaultParent, false);
            obj.gameObject.SetActive(false);

            m_hashes.Add(obj.GetInstanceID());
            m_elements.Enqueue(obj);
        }
    }

    public void Clear()
    {
        while (m_elements.Count > 0)
        {
            T element = m_elements.Dequeue();
            Object.Destroy(element);
        }
        m_hashes.Clear();
    }

    public T GetElement()
    {
        return GetElement(m_defaultParent);
    }

    public T GetElement(Transform newParent, bool worldPositionStays = true)
    {
        T element = null;

        if (newParent == null)
        {
            newParent = m_defaultParent;
        }

        if (m_elements.Count > 0)
        {
            element = m_elements.Dequeue() as T;
            m_hashes.Remove(element.GetInstanceID());
        }
        else
        {
            if (m_dynamicSize)
            {
                element = Object.Instantiate(m_prefab) as T;
                element.name = m_prefab.name;
                element.transform.SetParent(m_defaultParent, worldPositionStays);
            }
        }
        if (element != null)
        {
            element.transform.SetParent(newParent, worldPositionStays);
            element.gameObject.SetActive(true);
        }
        return element;
    }

    public void ReturnElement(T component, bool worldPositionStays = true)
    {
        if (m_hashes.Contains(component.GetInstanceID()))
        {
            return;
        }

        component.transform.SetParent(m_defaultParent, worldPositionStays);
        m_elements.Enqueue(component);
        m_hashes.Add(component.GetInstanceID());
        component.gameObject.SetActive(false);
    }
}