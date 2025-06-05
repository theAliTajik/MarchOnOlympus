using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InputToCommandHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    public Stat m_stat;

    public void plus()
    {
        int value = int.Parse(inputField.text);
        ModifyStatCommand plusCommand = new ModifyStatCommand(value, ModifierType.Additive, m_stat);
        CommandInvoker.ExecuteCommand(plusCommand);
    }

    public void minus()
    {
        
    }

    public void multiply()
    {
        
    }

    public void equals()
    {
        
    }
}
