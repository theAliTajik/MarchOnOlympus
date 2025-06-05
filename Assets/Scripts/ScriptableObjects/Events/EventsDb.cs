using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventsDb", menuName = "Olympus/EventsDb")]
public class EventsDb : GenericData<EventsDb>
{
    public List<EventInfo> EventInfos = new List<EventInfo>();
    
    public EventDialogueNode FinishEventDialogueNode;
    
    public EventDialogueNode FindByID(string eventId)
    {
        EventDialogueNode dialogueNode = null;
        
        for (var i = 0; i < EventInfos.Count; i++)
        {
            if (EventInfos[i].EventId == eventId)
            {
                dialogueNode = EventInfos[i].DialogueNode;
            }
        }

        if (dialogueNode == null)
        {
            Debug.Log("dialogue not found by id: " + eventId);
            return null;
        }
        
        return dialogueNode;
    }

    public EventDialogueNode GetRandomDialogue()
    {
        EventInfo randEvent = EventInfos[Random.Range(0, EventInfos.Count)];
        
        return randEvent.DialogueNode;
    }
}

[System.Serializable]
public struct EventInfo
{
    public string EventId;
    public EventDialogueNode DialogueNode;
}
