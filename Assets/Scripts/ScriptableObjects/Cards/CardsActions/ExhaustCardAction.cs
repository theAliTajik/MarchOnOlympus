using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExhaustCardAction : BaseCardAction
{
    private ExhaustCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ExhaustCard)cardData;

        int inventLevel = GameInfoHelper.GetInventLevel();
        int bleedAmount = inventLevel * m_data.InventLevelMultiplier;
        
        GameActionHelper.AddMechanicToFighter(target, bleedAmount, MechanicType.BLEED);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}