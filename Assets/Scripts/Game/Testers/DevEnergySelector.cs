#if UNITY_EDITOR



using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DevEnergySelector : MonoBehaviour
{
    public TMP_InputField text;
    
    private int CurrentValueSelected = 0;

    public void OnSliderValueChanged(float value)
    {
        CurrentValueSelected = (int)value;
        text.text = CurrentValueSelected.ToString();
    }

    public void OnButtonClicked()
    {
        int.TryParse(text.text, out CurrentValueSelected);
        ApplyValueToEnergy(CurrentValueSelected);
    }

    private void ApplyValueToEnergy(int value)
    {
        CombatManager.Instance.Energy.SetEnergy(value);
    }
}



#endif