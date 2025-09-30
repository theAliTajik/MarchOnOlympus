using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChaosProtects_PLUSCardAction : BaseCardAction
{
    private ChaosProtects_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ChaosProtects_PLUSCard)cardData;

        int sizeOfStartingDeck = GameInfoHelper.GetStartingDeckSize();

        int numOfRepeats = sizeOfStartingDeck / m_data.ForeachNumOfCardsInStartingDeck;

        if (numOfRepeats <= 0)
        {
            finishCallback?.Invoke();
            yield break;
        }

        int fortGain = m_data.Fortified * numOfRepeats;
        int strGain = m_data.Strength * numOfRepeats;
        int exileGain = m_data.Exile * numOfRepeats;
        
        GameActionHelper.AddMechanicToPlayer(fortGain, MechanicType.FORTIFIED);
        GameActionHelper.AddMechanicToPlayer(strGain, MechanicType.STRENGTH);
        //TODO: Exile
        // GameActionHelper.AddMechanicToPlayer(exileGain, MechanicType.EXILE);
        
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}