using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeshiftDefence_PLUSCardAction : BaseCardAction
{
    private MakeshiftDefence_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (MakeshiftDefence_PLUSCard)cardData;
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
        GameActionHelper.AddMechanicToPlayer(m_data.Fortified, MechanicType.FORTIFIED);
    }
}