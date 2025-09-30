using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeThemBleedMoreCardAction : BaseCardAction
{
    private MakeThemBleedMoreCard m_data;
    private Fighter m_target;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (MakeThemBleedMoreCard)cardData;
        
        m_target = target;

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if(phase != EGamePhase.PLAYER_TURN_START) return;

        ApplyBleed();
    }

    private void ApplyBleed()
    {
        int inventLevel = GameInfoHelper.GetInventLevel();
        if(inventLevel <= 0) return;
        
        GameActionHelper.AddMechanicToFighter(m_target, inventLevel, MechanicType.BLEED);
    }
}