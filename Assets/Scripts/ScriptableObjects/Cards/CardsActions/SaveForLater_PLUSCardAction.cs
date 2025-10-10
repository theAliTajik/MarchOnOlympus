using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveForLater_PLUSCardAction : BaseCardAction
{
    private SaveForLater_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (SaveForLater_PLUSCard)cardData;
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.Strength);
        
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