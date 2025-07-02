using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventPresenter : MonoBehaviour
{
    [SerializeField] private EventView m_eventView;
    
    [SerializeField] private EventDialogueNode m_testNode;
    [SerializeField] private CardDisplayList m_cardDisplayList;

    private void Start()
    {
        //display test node
        EventDialogueNode testNode = null;
        if (string.IsNullOrEmpty(GameSessionParams.EventId))
        {
            testNode = EventsDb.Instance.EventInfos[7].DialogueNode;
        }
        else
        {
            testNode = EventsDb.Instance.FindByID(GameSessionParams.EventId);
            Debug.Log($"found by id: {GameSessionParams.EventId}");
        }
        
        // testNode = EventsDb.Instance.EventInfos[6].DialogueNode;
        m_eventView.DisplayDialogue(testNode);
        m_eventView.OnEventChoiceClicked += OnEventChoiceClicked;
        GameplayEvents.ShowCardsByData += m_cardDisplayList.ShowCards;
    }

    private EventDialogueNode SelectRandomEvent()
    {
        EventDialogueNode randDialogue = EventsDb.Instance.GetRandomDialogue();
        return randDialogue;
    }

    private void OnEventChoiceClicked(EventChoiceData eventChoiceData)
    {
        if (eventChoiceData == null)
        {
            return;
        }

        StartCoroutine(EventChoiceClicked(eventChoiceData));
    }

    private IEnumerator EventChoiceClicked(EventChoiceData eventChoiceData)
    {
        yield return StartCoroutine(eventChoiceData.ExecuteActions());
        
        if (eventChoiceData.NextDialogueNode == EventsDb.Instance.FinishEventDialogueNode)
        {
            //finish event
            Debug.Log("event finished");
            EventController.Instance.FinishEvent();
            yield break;
        }
        
        if (eventChoiceData.NextDialogueNode == null)
        {
            // could be set to finish event
            Debug.Log("next dialogue is null");
            yield break;
        }
        m_eventView.DisplayDialogue(eventChoiceData.NextDialogueNode);
    }
}
