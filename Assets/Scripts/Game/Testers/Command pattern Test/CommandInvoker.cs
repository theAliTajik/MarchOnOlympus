using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    public static event Action CommandExecuted;
    
    private static List<ICommand> m_commandHistory = new List<ICommand>();
    
    public static List<ICommand> CommandHistory { get { return m_commandHistory; } }
    
    public static void ExecuteCommand(ICommand command)
    {
        command.Execute();
        m_commandHistory.Add(command);
        CommandExecuted?.Invoke();
    }
    
    public static void UndoCommand()
    {
        
    }
}