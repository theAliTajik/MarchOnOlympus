using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CommandDisplay : MonoBehaviour
{
    private ICommand m_command;
    
    public TMP_Text commandText;
    public GameObject UndoButton;
    
    public void Configure(ICommand command)
    {
        m_command = command;
        commandText.text = command.GetDescription();
    }

    public void OnUndoButtonClicked()
    {
        m_command.Undo();
        UndoButton.SetActive(false);
    }
}
