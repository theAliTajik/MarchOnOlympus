using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset;
    
    [SerializeField] private FighterHP m_fighterHp;
    [SerializeField] private Image m_bar;
    [SerializeField] private RectTransform m_barCap;
    [SerializeField] private TMP_Text m_currentHealth;
    [SerializeField] private TMP_Text m_maxHealth;

    private Fighter m_fighter;
    private int m_lastHealth;


    public void Config(Fighter fighter)
    {
        m_fighter = fighter;
        m_fighterHp = fighter.GetComponent<FighterHP>();
        m_fighterHp.OnHPChanged += OnHPChanged;
        SetHealth();
        setMaxHealth(m_fighterHp.Max);
    }
    
    public void Config(IHaveHP owner)
    {
        m_fighterHp = owner.GetHP();
        m_fighterHp.OnHPChanged += OnHPChanged;
        SetHealth();
        setMaxHealth(m_fighterHp.Max);
    }

    private void OnDestroy()
    {
        m_fighterHp.OnHPChanged -= OnHPChanged;
    }

    private void OnHPChanged()
    {
        SetHealth();
    }

    public void SetHealth()
    {
        m_bar.fillAmount = (float)m_fighterHp.Current / m_fighterHp.Max;
        if (m_barCap != null)
        {
            Vector2 v = m_barCap.anchoredPosition;
            v.x = m_bar.rectTransform.rect.width * m_bar.fillAmount;
        }

        m_currentHealth.text = m_fighterHp.Current.ToString();
        m_maxHealth.text = m_fighterHp.Max.ToString();
    }

    public void setMaxHealth(int health)
    {
        m_maxHealth.text = health.ToString();
    }
    
    public Vector3 GetOffset()
    {
        return m_offset;
    }
}
