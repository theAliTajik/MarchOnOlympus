using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MechanicWidget : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    [SerializeField] private TMP_Text m_text;

    private int m_count;

    public void Config(Sprite icon, int count)
    {
        m_icon.sprite = icon;
        SetCount(count);
    }

    public void SetCount(int count)
    {
        m_count = count;
        m_text.text = count.ToString();
    }
}