using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EventDialogueNode", menuName = "Events/EventDialogueNode")]
public class EventDialogueNode : ScriptableObject
{
    [TextArea]  
    public string DialogueText;
    
    public List<EventChoiceData> Choices = new List<EventChoiceData>();
    
}

