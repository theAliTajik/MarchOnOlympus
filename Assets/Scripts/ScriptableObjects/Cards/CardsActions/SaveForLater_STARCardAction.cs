using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveForLater_STARCardAction : BaseCardAction
{
    private SaveForLater_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (SaveForLater_STARCard)cardData;

        var arrowCards = GameInfoHelper.GetCardsWithName("Arrow", contains: true);
        int randCardIndex = UnityEngine.Random.Range(0, arrowCards.Count);
        GameActionHelper.DiscardCard(arrowCards[randCardIndex]);
        
        GameActionHelper.AddMechanicToPlayer(m_data.Strength, MechanicType.STRENGTH);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}