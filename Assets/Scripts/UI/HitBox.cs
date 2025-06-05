using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HitBox : MonoBehaviour
{
    
    [SerializeField] private Image m_boundaryImage;

    private void Start()
    {
        // hide hit box
        m_boundaryImage.color = new Color(m_boundaryImage.color.r, m_boundaryImage.color.g, m_boundaryImage.color.b, Mathf.Clamp01(0));
    }

    public void ShowHitBox(bool show)
    {
        if (show)
        {
            m_boundaryImage.color = new Color(m_boundaryImage.color.r, m_boundaryImage.color.g, m_boundaryImage.color.b, Mathf.Clamp01(256));
        }
        else
        {
            m_boundaryImage.color = new Color(m_boundaryImage.color.r, m_boundaryImage.color.g, m_boundaryImage.color.b, Mathf.Clamp01(0));
        }
    }

   
}
