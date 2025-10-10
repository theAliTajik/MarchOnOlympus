using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeditationCardAction : BaseCardAction
{
    private MeditationCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (MeditationCard)cardData;

        GameplayEvents.FighterRestoredHP += OnHealthRestore;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnHealthRestore(Fighter fighter, int amount)
    {
        if(!GameInfoHelper.CompareFighterToPlayer(fighter)) return;
        
        GameActionHelper.GainInvent(amount);
    }

    private void OnDestroy()
    {
        GameplayEvents.FighterRestoredHP -= OnHealthRestore;
    }
}