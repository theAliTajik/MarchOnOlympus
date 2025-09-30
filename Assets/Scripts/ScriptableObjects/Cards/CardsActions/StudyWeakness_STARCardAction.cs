using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class StudyWeakness_STARCardAction : BaseCardAction
{
    private StudyWeakness_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (StudyWeakness_STARCard)cardData;

        int debuffStack = GameInfoHelper.GetNumOfDebuffMechanics(target);

        if (debuffStack > 0)
        {
            GameActionHelper.AddMechanicToPlayer(m_data.Frenzy, MechanicType.FRENZY);
            GameplayEvents.SendOnGainInvent(m_data.Invent);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}