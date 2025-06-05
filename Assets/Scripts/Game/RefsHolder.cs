using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefsHolder
{
    private static Dictionary<System.Type, MonoBehaviour> m_refs = new Dictionary<System.Type, MonoBehaviour>();

    public static void Register<T>(T obj) where T:MonoBehaviour
    {
        System.Type type = typeof(T);
        if (!m_refs.TryAdd(type, obj))
        {
            m_refs[type] = obj;    
        }
    }

    public static T Get<T>() where T:MonoBehaviour
    {
        System.Type type = typeof(T);
        if (m_refs.ContainsKey(type))
        {
            return (T)m_refs[typeof(T)];
        }

        return null;
    }
    
    public static void SetNull<T>(T obj = null) where T:MonoBehaviour
    {
        System.Type type = typeof(T);
        m_refs.Remove(type);
    }
}