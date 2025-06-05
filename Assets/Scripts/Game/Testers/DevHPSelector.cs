using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DevHPSelector : MonoBehaviour
{
    [SerializeField] private FighterSelectorDropDown m_fighterSelectorDropDown;
    public TMP_InputField text;
    
    public int CurrentValueSelected = 0;

    public void OnSliderValueChanged(float value)
    {
        CurrentValueSelected = (int)value;
        text.text = CurrentValueSelected.ToString();
    }

    public void OnButtonClicked()
    {
        if (string.IsNullOrWhiteSpace(text.text))
        {
            return;
        }
        int.TryParse(text.text, out CurrentValueSelected);
        Fighter fighter = m_fighterSelectorDropDown.GetSelectedFighter();
        ApplyValueToFighterHP(fighter, CurrentValueSelected);
    }

    private void ApplyValueToFighterHP(Fighter fighter, int value)
    {
        int currentHP = fighter.HP.Current;
        if (value > currentHP)
        {
            fighter.Heal(value - currentHP);
        }
        else
        {
            fighter.TakeDamage(currentHP - value, CombatManager.Instance.Player, false);
        }
    }
}
