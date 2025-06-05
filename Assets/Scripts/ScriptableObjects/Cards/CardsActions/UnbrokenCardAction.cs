using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnbrokenCardAction : BaseCardAction
{
    private UnbrokenCard m_data;
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (UnbrokenCard)cardData;
        GameplayEvents.GamePhaseChanged += OnPhaseChanged;
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChanged(EGamePhase phase)
    {
        switch (phase)
        {
            case EGamePhase.CARD_DRAW_FINISHED:
                int BlockStr = GameInfoHelper.GetMechanicStack(GameInfoHelper.GetPlayer(), MechanicType.BLOCK);
                if (BlockStr > 0)
                {
                    GameActionHelper.AddMechanicToPlayer(m_data.StrGain, MechanicType.STRENGTH);
                }
                break;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
    }
}