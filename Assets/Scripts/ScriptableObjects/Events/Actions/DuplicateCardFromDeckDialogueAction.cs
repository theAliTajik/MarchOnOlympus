
using UnityEngine;

[CreateAssetMenu(fileName = "DuplicateCardAction", menuName = "Events/DialogueActions/DuplicateCardAction")]
public class DuplicateCardFromDeckDialogueAction : DialogueAction
{
    private DialogueContex m_context;

    public override void Execute(DialogueContex context)
    {
        base.Execute(context);

        if (!string.IsNullOrEmpty(GameSessionParams.DeckTemplateClientId))
        {
            DeckTemplates.Deck template = DeckTemplates.FindById(GameSessionParams.DeckTemplateClientId);
            GameplayEvents.SendShowCardsForSelecting(template.CardsInDeck);
            GameplayEvents.CardSelectedByPlayer += OnCardSelected;
        }
        else
        {
            Debug.Log("deck template id was null");
        }

        m_context = context;
    }
    
    
    private void OnCardSelected(CardDisplay cardDisplay)
    {
        Debug.Log("selected this card: " + cardDisplay.CardInDeck.GetCardName());

        CardInDeckStateMachine duplicatecard = cardDisplay.CardInDeck.Clone();
        DeckTemplates.AddCardToDeck(GameSessionParams.DeckTemplateClientId, duplicatecard, -1);
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;   
        EventController.Instance.FinishEvent();
    }
}
