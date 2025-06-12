
using Game.ModifiableParam;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeCardCostAction", menuName = "Events/DialogueActions/ChangeCardCostAction")]
public class ChangeCardCostDialogueAction : DialogueAction
{
    private DialogueContex m_context;
    [SerializeField] private int m_cardCost;

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
        CardInDeckStateMachine card = cardDisplay.CardInDeck;
        Debug.Log("selected this card: " + card.GetCardName());
        IParamModifier<int> setEnergy = new SetValueModifier<int>(m_cardCost);
        card.SetEnergyOverride(setEnergy);
        DeckTemplates.Save();
        GameplayEvents.CardSelectedByPlayer -= OnCardSelected;   
        EventController.Instance.FinishEvent();
    }
}
