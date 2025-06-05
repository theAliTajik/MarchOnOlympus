using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnergyTester : MonoBehaviour
{
    public Energy m_energy;
    public EnergyWidget m_EnergyWidget;
    public Button m_ChangeEnergyButton;
    public Button m_SetEnergyButton;
    public Toggle m_EffectMaxEnergyToggle;
    public TMP_InputField m_InputAmount;

    void Start()
    {
        m_ChangeEnergyButton.onClick.AddListener(OnChangeEnergyButtonPressed);
        m_SetEnergyButton.onClick.AddListener(OnSetEnergyButtonPressed);
        m_EnergyWidget.SetCurrentEnergy(m_energy.Current);
        m_EnergyWidget.SetMaxEnergy(m_energy.Max);
    }

    void OnChangeEnergyButtonPressed()
    {
        int amount;
        if (!int.TryParse(m_InputAmount.text, out amount))
        {
            Debug.LogError("Invalid energy input.");
            return;
        }

        if (m_EffectMaxEnergyToggle.isOn)
        {
            m_energy.ChangeMaxEnergy(amount);
            m_EnergyWidget.SetMaxEnergy(m_energy.Max);
        }
        else
        {
            m_energy.GainEnergy(amount);
            m_EnergyWidget.SetCurrentEnergy(m_energy.Current);
        }
       
    }
    void OnSetEnergyButtonPressed()
    {
        int amount;
        if (!int.TryParse(m_InputAmount.text, out amount))
        {
            Debug.LogError("Invalid energy input.");
            return;
        }

        if (m_EffectMaxEnergyToggle.isOn)
        {
            m_energy.SetMaxEnergy(amount);
            m_EnergyWidget.SetMaxEnergy(m_energy.Max);
        }
        else
        {
            m_energy.SetEnergy(amount);
            m_EnergyWidget.SetCurrentEnergy(m_energy.Current);
        }
     
    }
 
  
}