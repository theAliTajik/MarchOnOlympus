
using System;
using System.Collections.Generic;
using System.Linq;
using Game.ModifiableParam;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class ModifiableParam<T>
{
    public ModifiableParam()
    {
        
    }

    public ModifiableParam(T defalutValue)
    {
        m_value = defalutValue;
    }
    public T Value
    {
        get { return GetValue(); }
        set { m_value = value; }
    }

    public T OriginalValue
    {
        get { return m_value; }
    }

    [SerializeField] [JsonProperty] private T m_value;
    [JsonProperty] private List<IParamModifier<T>> m_modifiers = new List<IParamModifier<T>>();

    private T GetValue()
    {
        if (m_modifiers.Count <= 0)
        {
            return m_value;
        }
        
        // apply stack modifiers
        T result = m_value;
        foreach (IParamModifier<T> paramModifier in m_modifiers.OrderBy(m => m.Priority))
        {
            result = paramModifier.Modify(result);
        }
        return result;
    }

    public void AddModifier(IParamModifier<T> modifier)
    {
        m_modifiers.Add(modifier);
    }

    public void RemoveModifier(IParamModifier<T> modifier)
    {
        m_modifiers.Remove(modifier);
    }
    
    public static implicit operator T(ModifiableParam<T> param)
    {
        return param.Value;
    }
    
    public static implicit operator ModifiableParam<T>(T value)
    {
        return new ModifiableParam<T> { Value = value };
    }

    public void RemoveAllModifiers()
    {
        m_modifiers.Clear();
    }

    public void LogAllModifiers()
    {
        Debug.Log($"Logging {m_modifiers.Count} modifiers:");
        foreach (var modifier in m_modifiers)
        {
            Debug.Log("modifier of type: " + modifier.GetType());
        }
    }
    
    public ModifiableParam<T> Clone()
    {
        var copy = new ModifiableParam<T>(m_value);
        foreach (var modifier in m_modifiers)
        {
            copy.AddModifier(modifier);
        }
        return copy;
    }
}

