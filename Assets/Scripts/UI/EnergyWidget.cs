using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
public class EnergyWidget : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private TMP_Text m_CurrentEnergy;
    [SerializeField] private TMP_Text m_MaxEnergy;
    
    public void SetCurrentEnergy(int currentEnergy)
    {
        m_CurrentEnergy.text = currentEnergy.ToString();
    }

    public void SetMaxEnergy(int maxEnergy)
    {
        m_MaxEnergy.text = maxEnergy.ToString();
    }

    public void SetAnimation(bool setActive)
    {
        if (setActive)
        {
            m_animator.SetTrigger("Activate");    
        }
        else
        {
            m_animator.SetTrigger("Deactivate");    
        }
    }
}
