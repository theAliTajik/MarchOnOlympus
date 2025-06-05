using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContex
{
    public EventDialogueNode NextDialogueNode;
}

public interface IDialogueAction
{
    void Execute(DialogueContex context);
}

public abstract class DialogueAction : ScriptableObject, IDialogueAction
{
    public virtual void Execute(DialogueContex context)
    {
    }
}