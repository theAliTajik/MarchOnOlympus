using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChaosProtects_STARCardAction : BaseCardAction
{
    private ChaosProtects_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (ChaosProtects_STARCard)cardData;

        int numOfRepeates = GameInfoHelper.GetNumOfStartingDeckChunks(m_data.ForEeachNumOfCardsInStartingDeck);
        for (int i = 0; i < numOfRepeates; i++)
        {
            int damage = UnityEngine.Random.Range(m_data.DamageButtomRange, m_data.DamageTopRange + 1);
            GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        }
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}