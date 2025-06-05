using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventView : MonoBehaviour
{
    public Action<EventChoiceData> OnEventChoiceClicked;
    
    [SerializeField] private TMP_Text m_dialogueBodyText;
    [SerializeField] private GameObject m_choicesContentContainer;
    
    [SerializeField] private EventChoiceDisplay m_choiceDisplayPrefab;
    
    private List<EventChoiceDisplay> m_choices = new List<EventChoiceDisplay>();
    
    private EventDialogueNode m_currentDialogue;

    public void DisplayDialogue(EventDialogueNode node)
    {
        m_currentDialogue = node;
        m_dialogueBodyText.text = node.DialogueText;

        List<EventChoiceData> dialogueChoices = node.Choices;

        
        for (int i = 0; i < dialogueChoices.Count; i++)
        {
            EventChoiceDisplay choiceDisplay;

            if (i < m_choices.Count) // reuse choiceDisplay if available
            {
                choiceDisplay = m_choices[i];
            }
            else 
            {
                choiceDisplay = Instantiate(m_choiceDisplayPrefab, m_choicesContentContainer.transform);
                m_choices.Add(choiceDisplay);
            }

            choiceDisplay.gameObject.SetActive(true);
            
            string title = dialogueChoices[i].Title;
            string choiceText = dialogueChoices[i].Description;
            
            choiceDisplay.Configure(title, choiceText, dialogueChoices[i]);
            choiceDisplay.OnClicked += ChoiceClicked;
        }

        // Deactivate unused displays
        for (int i = dialogueChoices.Count; i < m_choices.Count; i++)
        {
            m_choices[i].gameObject.SetActive(false);
        }
    }


    public void ChoiceClicked(EventChoiceData data)
    {
        Debug.Log("choice id: " + data.Title);
        OnEventChoiceClicked?.Invoke(data);
    }
}
