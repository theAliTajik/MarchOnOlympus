using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChaosProtectsCardAction : BaseCardAction
{
    private ChaosProtectsCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ChaosProtectsCard)cardData;

        int numOfStartingCards = GameInfoHelper.GetStartingDeckSize();
        int numOfRepeats = numOfStartingCards / m_data.ForeachNumOfCards;

        if (numOfRepeats <= 0)
        {
            finishCallback?.Invoke();
            yield break;
        }

        int fortifiedGain = m_data.Fortified * numOfRepeats;
        int ExileGain = m_data.Exile * numOfRepeats;
        int IngeniusGain = m_data.Ingenius * numOfRepeats;

        GameActionHelper.AddMechanicToPlayer(fortifiedGain, MechanicType.FORTIFIED);
        //TODO: EXILE
        // GameActionHelper.AddMechanicToPlayer(ExileGain, MechanicType.EXILE);
        GameActionHelper.AddMechanicToPlayer(IngeniusGain, MechanicType.INGENIUS);
                
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}