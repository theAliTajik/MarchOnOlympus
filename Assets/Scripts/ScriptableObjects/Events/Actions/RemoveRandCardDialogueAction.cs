
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveRandCardAction", menuName = "Events/DialogueActions/RemoveRandCardAction")]
public class RemoveRandCardDialogueAction : DialogueAction
{
        public override void Execute(DialogueContex context)
        {
            string currentdeckId = null;
            if (string.IsNullOrWhiteSpace(GameSessionParams.DeckTemplateClientId))
            {
                Debug.Log("null dedck id");
                return;
            }

            currentdeckId = GameSessionParams.DeckTemplateClientId;
            //pick rand card
            DeckTemplates.Deck deck = DeckTemplates.FindById(currentdeckId);
            CardInDeckStateMachine randCard = deck.CardsInDeck[UnityEngine.Random.Range(0, deck.CardsInDeck.Count)];
            DeckTemplates.RemoveCardFromDeck(currentdeckId, randCard);
        }
}
