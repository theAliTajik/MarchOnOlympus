using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    #region Singleton

    private static EventManager _sInstance;
    public static EventManager Instance => _sInstance;

    void Awake()
    {
        _sInstance = this;
    }

    void OnDestroy()
    {
        _sInstance = null;
    }
    #endregion
   
    // Dictionary of event types and their subscribers
    private static Dictionary<string, Action<object>> m_eventDictionary = new Dictionary<string, Action<object>>();

    // Subscribe to an event
    public static void Subscribe(string eventName, Action<object> listener)
    {
        if (m_eventDictionary.ContainsKey(eventName))
        {
            m_eventDictionary[eventName] += listener;
        }
        else
        {
            m_eventDictionary.Add(eventName, listener);
        }
    }

    // Unsubscribe from an event
    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        if (m_eventDictionary.ContainsKey(eventName))
        {
            m_eventDictionary[eventName] -= listener;
        }
    }

    // Publish an event
    public static void Publish(string eventName, object eventData = null)
    {
        if (m_eventDictionary.ContainsKey(eventName))
        {
            m_eventDictionary[eventName]?.Invoke(eventData);
        }
    }
}
