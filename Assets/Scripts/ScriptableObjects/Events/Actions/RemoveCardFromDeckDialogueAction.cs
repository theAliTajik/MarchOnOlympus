
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveCardAction", menuName = "Events/DialogueActions/RemoveCardAction")]
public class RemoveCardFromDeckDialogueAction : DialogueAction
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
        DeckTemplates.RemoveCardFromDeck(GameSessionParams.DeckTemplateClientId, cardDisplay.CardInDeck);
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;   
        EventController.Instance.FinishEvent();
    }
}
