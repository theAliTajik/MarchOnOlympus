using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventChoiceDisplay : MonoBehaviour, IPointerClickHandler
{
    public Action<EventChoiceData> OnClicked;
    
    [SerializeField] private TMP_Text m_titleText;
    [SerializeField] private TMP_Text m_choiceText;

    private EventChoiceData m_choiceData;

    public void Configure(string title, string choice, EventChoiceData data)
    {
        m_titleText.text = title;
        m_choiceText.text = choice;
        m_choiceData = data;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_choiceData != null)
        {
            OnClicked?.Invoke(m_choiceData);
        }
    }
}
