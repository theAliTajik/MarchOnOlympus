using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnEnterDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnEnter;
    public event Action OnExit;
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit?.Invoke();
    }
}
