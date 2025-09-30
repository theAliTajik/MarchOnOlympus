using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class RendingMechanismCardAction : BaseCardAction
{
    private RendingMechanismCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (RendingMechanismCard)cardData;

        int inventionsThisMatch = GameInfoHelper.GetInventionsThisMatch();

        if (inventionsThisMatch > 0)
        {
            int bleed = m_data.Bleed * inventionsThisMatch;
            GameActionHelper.AddMechanicToFighter(target, bleed, MechanicType.BLEED);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}