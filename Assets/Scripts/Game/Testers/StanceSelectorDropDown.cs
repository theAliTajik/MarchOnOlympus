using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

public class StanceSelectorDropDown : MonoBehaviour
{
    public TMP_Dropdown Dropdown;
    
    public void OnValueChanged()
    {
        CombatManager.Instance.OnStanceSelected(GetSelectedStance(Dropdown.value));
    }

    private Stance GetSelectedStance(int value)
    {
        if (System.Enum.IsDefined(typeof(Stance), value))
        {
            return (Stance)value;
        }
        else
        {
            Debug.LogError("Invalid value for Stance enum!");
            return default;
        }
    }
}
