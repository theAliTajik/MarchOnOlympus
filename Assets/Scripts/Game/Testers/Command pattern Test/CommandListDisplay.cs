using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandListDisplay : MonoBehaviour
{
    public CommandDisplay m_displayPrefab;
    
    public List<CommandDisplay> m_commands = new List<CommandDisplay>();

    private void Start()
    {
        CommandInvoker.CommandExecuted += OnListChange;
        OnListChange();
    }

    private void OnDestroy()
    {
        CommandInvoker.CommandExecuted -= OnListChange;
    }

    private void OnListChange()
    {
        int countDiff = CommandInvoker.CommandHistory.Count - m_commands.Count;
        if (countDiff >= 0)
        {
            for (int i = 0; i < countDiff; i++)
            {
                SpawnPrefab();
            }
        }
        else
        {
            for (int i = 0; i < -countDiff; i++)
            {
                CommandDisplay removeCommand = m_commands[m_commands.Count - 1];
                m_commands.Remove(removeCommand);
                Destroy(removeCommand.gameObject);
            }
        }

        for (var i = 0; i < m_commands.Count; i++)
        {
            m_commands[i].Configure(CommandInvoker.CommandHistory[i]);
        }
    }

    public void SpawnPrefab()
    {
        CommandDisplay instance = Instantiate(m_displayPrefab, transform);
        //instance.transform.SetParent(transform);
        m_commands.Add(instance);
    }
}
