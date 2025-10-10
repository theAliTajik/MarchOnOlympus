using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoomerangCardAction : BaseCardAction
{
    private BoomerangCard m_data;
    private int numOfTimesDiscarded;

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BoomerangCard)cardData;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    public override void Discarded(BaseCardData cardData)
    {
        base.Discarded(cardData);

        int damage = m_data.Damage;
        damage += numOfTimesDiscarded * m_data.ExtraDamageEachTimeDiscarded;
        
        numOfTimesDiscarded++;

        var randEnemy = GameInfoHelper.GetRandomEnemy();
        GameActionHelper.DamageFighter(randEnemy, GameInfoHelper.GetPlayer(), damage);

    }
}