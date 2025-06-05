using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutsmartCardAction : BaseCardAction
{
    private Fighter m_target;
    private bool m_stanceActive;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        OutsmartCard c = (OutsmartCard)cardData;
        
        //enemy uses abillity twice
        GameActionHelper.MakeEnemyPlayTwice(target);
        
        //apply stun
        m_target = target;
        GameplayEvents.GamePhaseChanged += OnPhaseChanged;

        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            // draw 2 next turn
            m_stanceActive = true;
            GameActionHelper.SetDrawAmountOverride(2, true);
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChanged(EGamePhase phase)
    {
        if (phase == EGamePhase.CARD_DRAW_FINISHED)
        {
            GameActionHelper.AddMechanicToFighter(m_target, 1, MechanicType.STUN);
            if (m_stanceActive)
            {
                GameActionHelper.RemoveCardDrawAmountOverride();
            }
            GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
    }
}