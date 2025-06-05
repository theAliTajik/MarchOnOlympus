using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WellRoundedWarriorCardAction : BaseCardAction
{
    private const float m_dealyBetweenEachCardPlay = 0.3f;
    List<CardDisplay> cardsToPlay = new List<CardDisplay>();
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        WellRoundedWarriorCard c = (WellRoundedWarriorCard)cardData;
        
        // get all cards
        int numOfBerserkerCards = GameInfoHelper.CountCardsByStance(Stance.BERSERKER, CardStorage.ALL);
        
        if (numOfBerserkerCards <= 0)
        {
            foreach (var storage in new[] {CardStorage.DRAW_PILE, CardStorage.HAND})
            {
                if (cardsToPlay.Count >= c.NumOfCardsToPlay) break;

                cardsToPlay.AddRange(GameInfoHelper.GetAllCardsIn(storage));
            }

            if (cardsToPlay.Count < c.NumOfCardsToPlay)
            {
                Debug.Log("Not enough cards to play");
            }

            int numOfCardsToPlay = Mathf.Min(cardsToPlay.Count, c.NumOfCardsToPlay);

            StartCoroutine(playCards(numOfCardsToPlay));
        }
        
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private IEnumerator playCards(int numOfCardsToPlay)
    {
        for (int i = 0; i < numOfCardsToPlay; i++)
        {
            yield return new WaitForSeconds(m_dealyBetweenEachCardPlay);
            GameActionHelper.SetCardEnergyOverride(cardsToPlay[i], ECardInDeckState.NORMAL, 0);
            GameActionHelper.SetCardEnergyOverride(cardsToPlay[i], ECardInDeckState.STANCE, 0);
            GameActionHelper.PlayCard(cardsToPlay[i], GameInfoHelper.GetRandomEnemy());
        }
    }

}