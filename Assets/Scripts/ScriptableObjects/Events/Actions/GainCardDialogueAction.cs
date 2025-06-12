
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GainCardAction", menuName = "Events/DialogueActions/GainCardAction")]
public class GainCardDialogueAction : DialogueAction
{
    [SerializeField] private BaseCardData m_cardData;

    public override void Execute(DialogueContex context)
    {
        //Add card data to deck
        string currentdeckId = null;
        if (!string.IsNullOrWhiteSpace(GameSessionParams.DeckTemplateClientId))
        {
            currentdeckId = GameSessionParams.DeckTemplateClientId;
        }
        
        if (!string.IsNullOrEmpty(currentdeckId))
        {
            DeckTemplates.AddCardToDeck(currentdeckId, m_cardData.Name, 0);
            Debug.Log("card added: " + m_cardData.Name);
        }
    }
}
