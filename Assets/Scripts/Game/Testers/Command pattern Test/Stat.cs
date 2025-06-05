using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stat : MonoBehaviour
{

    public TMP_Text text;
    
    public int DefaultValue;
    
    private int ValueOverride = -1;

    
    
    public int GetValue()
    {
        if (ValueOverride <= -1)
        {
            return DefaultValue;
        }
        
        return ValueOverride;
    }
    
    public void SetValueOverride(int newValue, bool additive = false)
    {
        if (additive)
        {
            ValueOverride = newValue + GetValue();
            if (ValueOverride < 0)
            {
                ValueOverride = 0;
            }
            return;
        }

        ValueOverride = newValue;
    }

    public void RemoveEnergyOverride()
    {
        ValueOverride = -1;
    }

    private void Update()
    {
        text.text = GetValue().ToString();
    }
}
