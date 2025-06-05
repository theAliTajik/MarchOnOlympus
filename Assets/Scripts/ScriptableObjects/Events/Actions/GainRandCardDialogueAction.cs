
using UnityEngine;

[CreateAssetMenu(fileName = "GainRandCardAction", menuName = "Events/DialogueActions/GainRandCardAction")]
public class GainRandCardDialogueAction : DialogueAction
{
        
        public override void Execute(DialogueContex context)
        {
            if (string.IsNullOrWhiteSpace(GameSessionParams.deckTemplateClientId))
            {
                Debug.Log("current deck id is null");
                return;
            }

            string currentdeckId = GameSessionParams.deckTemplateClientId;
                
            //pick rand card
            BaseCardData cardData = CardsDb.Instance.GetRandom();
                
            DeckTemplates.AddCardToDeck(currentdeckId, cardData, 0);
            Debug.Log("card added: " + cardData);
        }
}
