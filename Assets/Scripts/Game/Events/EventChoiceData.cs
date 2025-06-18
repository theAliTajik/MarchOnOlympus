using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EventChoiceData", menuName = "Events/EventChoiceData")]
public class EventChoiceData : ScriptableObject
{
    public string Title;
    public string Description;

    public List<ScriptableObject> ActionAssets = new List<ScriptableObject>();
    
    public EventDialogueNode NextDialogueNode;

    public IEnumerator ExecuteActions()
    {
        for (int i = 0; i < ActionAssets.Count; i++)
        {
            if (ActionAssets[i] is IDialogueAction action)
            {
                ExecuteAction(action);
                //yield return new WaitForSeconds();
            }
            else
            {
                Debug.Log("DialogueNode action asset is not a dialogue action.");
            }
        }
        yield break;
    }

    private void ExecuteAction(IDialogueAction action)
    {
        DialogueContex context = new DialogueContex();
        context.NextDialogueNode = NextDialogueNode;
                    
        action.Execute(context);
    }

}
