using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class StudyWeaknessCardAction : BaseCardAction
{
    private StudyWeaknessCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (StudyWeaknessCard)cardData;

        int enemyDebuffCount = GameInfoHelper.GetNumOfDebuffMechanics(target);

        if (enemyDebuffCount > 0)
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