using System;
using System.Collections;
using System.Collections.Generic;
using Game.ModifiableParam;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class Energy : MonoBehaviour
{
    private ModifiableParam<int> m_Max = new ModifiableParam<int>();
    private int m_Current;
    
    public int Max => m_Max;
    public int Current => m_Current;
    
    
    
    [SerializeField] private EnergyWidget m_energyWidget;

    private void Start()
    {
        setCurrentToMax();
    }

    public void setCurrentToMax()
    {
        m_Current = m_Max;
        UpdateEnergyWidget();
    }
    
    public void GainEnergy(int amount)
    {
        if (m_Current + amount <= 0)
        {
            m_Current = 0;
            UpdateEnergyWidget();
            return;
        }
        m_Current += amount;
        UpdateEnergyWidget();
    }

    public void UseEnergy(int amount)
    {
        if (m_Current - amount <= 0)
        {
            m_Current = 0;
            UpdateEnergyWidget();
            return;
        }
        m_Current -= amount;
        UpdateEnergyWidget();
    }

    public void ChangeMaxEnergy(int amount)
    {
        if (m_Max + amount <= 0)
        {
            m_Max = 0;
            UpdateEnergyWidget();
            return;
        }
        m_Max += amount;
        UpdateEnergyWidget();
    }

    public void ResetEnergy()
    {
        m_Current = m_Max;
        UpdateEnergyWidget();
    }

    public void SetEnergy(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Invalid amount of energy (energy lower than 0), energy set to 0");
            m_Current = 0;
            return;
        }
        m_Current = amount;
        UpdateEnergyWidget();
    }
    
    public void SetMaxEnergy(int amount)
    {
        if (amount < 0)
        {
            CustomDebug.LogWarning("Invalid amount of energy (energy lower than 0), max energy set to 0", Categories.Combat.Energy);
            m_Max = 0;
            return;
        }
        m_Max = amount;
        CustomDebug.Log($"set max energy {m_Max.Value}", Categories.Combat.Energy);
        UpdateEnergyWidget();
    }

    public void UpdateEnergyWidget()
    {
        m_energyWidget.SetCurrentEnergy(m_Current);
        m_energyWidget.SetMaxEnergy(m_Max);
        GameplayEvents.SendOnEnergyChanged(m_Current);
    }

    public void AddModifier(IParamModifier<int> modifier)
    {
        Debug.Log($"added modifier {modifier}");
        m_Max.AddModifier(modifier);
        UpdateEnergyWidget();
    }

    public void RemoveModifier(IParamModifier<int> modifier)
    {
        m_Max.RemoveModifier(modifier);
        UpdateEnergyWidget();
    }
}
