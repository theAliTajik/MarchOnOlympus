using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuryCardAction : BaseCardAction
{
    private FuryCard m_data;
    private int NumOfTurns = 0;
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (FuryCard)cardData;

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if (phase == EGamePhase.CARD_DRAW_FINISHED)
        {
            if (NumOfTurns < m_data.NumOfTurnsTrigger)
            {
                GameActionHelper.AddMechanicToPlayer(m_data.StrPerTurn, MechanicType.STRENGTH);
                NumOfTurns++;
            }
            else
            {
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), GameInfoHelper.GetPlayer(), m_data.SelfDamage);
                GameplayEvents.GamePhaseChanged -= OnPhaseChange;
            }
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }
}