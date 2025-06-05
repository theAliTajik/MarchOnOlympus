using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EntityHPTester : MonoBehaviour
{
    [FormerlySerializedAs("m_entityHP")] public FighterHP m_fighterHp;
    public TMP_Text m_hpDisplay;
    public Button m_takeDamageButton;
    public Button m_healButton;
    public Button m_resetHPButton;
    public TMP_InputField m_damageInput;
    public TMP_InputField m_healInput;

    void Start()
    {
        m_takeDamageButton.onClick.AddListener(OnTakeDamageButtonPressed);
        m_healButton.onClick.AddListener(OnHealButtonPressed);
        m_resetHPButton.onClick.AddListener(OnResetHPButtonPressed);
        UpdateHPDisplay();
    }

    void OnTakeDamageButtonPressed()
    {
        int damage;
        if (!int.TryParse(m_damageInput.text, out damage))
        {
            Debug.LogError("Invalid damage input.");
            return;
        }
        bool isFatal = m_fighterHp.IsDamageFatal(damage);  
        m_fighterHp.TakeDamage(damage);
        UpdateHPDisplay();

        // Log whether the damage was fatal
        if (isFatal)
        {
            Debug.Log("Damage was fatal!");
        }
        else
        {
            Debug.Log("Damage was not fatal.");
        }
    }

    void OnHealButtonPressed()
    {
        int heal;
        if (int.TryParse(m_healInput.text, out heal))
        {
            m_fighterHp.Heal(heal);
            UpdateHPDisplay();
        }
        else
        {
            Debug.LogError("Invalid heal input.");
        }
    }

    void OnResetHPButtonPressed()
    {
        m_fighterHp.ResetHP();
        UpdateHPDisplay();
    }

    void UpdateHPDisplay()
    {
        m_hpDisplay.text = "Current HP: " + m_fighterHp.Current;
    }
}