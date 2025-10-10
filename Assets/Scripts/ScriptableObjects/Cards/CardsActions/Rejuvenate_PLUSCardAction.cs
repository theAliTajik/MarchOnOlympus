using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.ChangeTrackerService;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rejuvenate_PLUSCardAction : BaseCardAction
{
    private Rejuvenate_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Rejuvenate_PLUSCard)cardData;
        PerformAction();
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    public override void Discarded(BaseCardData cardData)
    {
        base.Discarded(cardData);
        PerformAction();
    }

    private void PerformAction()
    {
        GameActionHelper.HealPlayer(m_data.Restore);
        GameActionHelper.GainInvent(m_data.Invent);
    }
}