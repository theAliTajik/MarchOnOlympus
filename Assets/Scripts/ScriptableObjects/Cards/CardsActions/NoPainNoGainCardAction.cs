using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoPainNoGainCardAction : BaseCardAction
{
    private NoPainNoGainCard m_data; 
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        m_data = (NoPainNoGainCard)cardData;
        GameplayEvents.GamePhaseChanged += OnPhaseChanged;

        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChanged(EGamePhase phase)
    {
        if (phase == EGamePhase.PLAYER_DAMAGED && GameInfoHelper.IsPlayerTurn())
        {
            GameActionHelper.AddMechanicToPlayer(m_data.StrGain, MechanicType.STRENGTH);
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
    }
}