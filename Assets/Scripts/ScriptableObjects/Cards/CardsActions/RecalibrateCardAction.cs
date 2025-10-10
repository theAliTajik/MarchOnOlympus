using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecalibrateCardAction : BaseCardAction
{
    private RecalibrateCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (RecalibrateCard)cardData;
        GameplayEvents.OnCardDiscarded += OnCardDiscarded;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnCardDiscarded(CardDisplay card)
    {
        GainInvent();
    }

    private void GainInvent()
    {
        GameplayEvents.SendOnGainInvent(m_data.Invent);
    }
}