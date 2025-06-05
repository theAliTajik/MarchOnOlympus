using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : MonoBehaviour
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }

    [SerializeField] private StringEvent m_action1;
    [SerializeField] private StringEvent m_action2;
    [SerializeField] private StringEvent m_action3;


    public void CallEvent1(string param)
    {
        m_action1?.Invoke(param);
    }

    public void CallEvent2(string param)
    {
        m_action2?.Invoke(param);
    }

    public void CallEvent3(string param)
    {
        m_action3?.Invoke(param);
    }
}
