using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class EndTurnWidget : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private GameObject m_button;

    public void SetState(bool value)
    {
        
        if (m_animator != null && m_button != null)
        {
            m_animator.SetBool("On", value);
            m_button.SetActive(value);
        }
        else
        {
            Debug.Log("eneregy widget animator null");
        }
    }

    private void Start()
    {
        GameplayEvents.ChangeEndTurnButtonState += SetState;
    }

    private void OnDestroy()
    {
        GameplayEvents.ChangeEndTurnButtonState -= SetState;
    }
}
