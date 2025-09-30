using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreativeChaos_PLUSCardAction : BaseCardAction
{
    private CreativeChaos_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (CreativeChaos_PLUSCard)cardData;
        int startingDeckSize = GameInfoHelper.GetStartingDeckSize();

        int inventGain = startingDeckSize + m_data.ExtraInvent;
        
        GameplayEvents.SendOnGainInvent(inventGain);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}