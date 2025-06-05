using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FinaleCardAction : BaseCardAction
{
    private FinaleCard m_data;
    private Fighter m_target;

    private int m_damageDoneBeforeCard = 0;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (FinaleCard)cardData;
        m_target = target;

        m_damageDoneBeforeCard = GameInfoHelper.GetDamageDoneToEnemiesOverAll();
        
        GameplayEvents.GamePhaseChanged += OnPhaseChanged;
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChanged(EGamePhase phase)
    {
        if (phase == EGamePhase.ENEMY_DAMAGED)
        {
            int damageDone = GameInfoHelper.GetDamageDoneToEnemiesOverAll() - m_damageDoneBeforeCard;
            if (damageDone >= m_data.DamageDoneThreshold)
            {
                Fighter randTarget = GameInfoHelper.GetRandomEnemy();
                m_damageDoneBeforeCard = GameInfoHelper.GetDamageDoneToEnemiesOverAll();
                GameActionHelper.DamageFighter(randTarget, GameInfoHelper.GetPlayer(), m_data.Damage);
            }
        }
    }
}