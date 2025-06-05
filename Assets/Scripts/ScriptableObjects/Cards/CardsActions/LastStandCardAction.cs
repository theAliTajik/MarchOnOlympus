using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LastStandCardAction : BaseCardAction
{
    public int restoreAmount = 0;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        LastStandCard c = (LastStandCard)cardData;
        
        CombatManager.Instance.Player.HP.SetGuard(c.GuardMin);
        GameplayEvents.GamePhaseChanged += OnTurnEnd;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            restoreAmount = c.StanceRestoreNextTurn; 
            GameplayEvents.GamePhaseChanged += OnNextTurn;
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    public void OnTurnEnd(EGamePhase gamePhase)
    {
        if (gamePhase == EGamePhase.ENEMY_TURN_END)
        {
            CombatManager.Instance.Player.HP.RemoveGuard();
            GameplayEvents.GamePhaseChanged -= OnTurnEnd;
        }
    }

    public void OnNextTurn(EGamePhase gamePhase)
    {
        if (gamePhase == EGamePhase.PLAYER_TURN_START)
        {
            CombatManager.Instance.Player.Heal(restoreAmount);
            GameplayEvents.GamePhaseChanged -= OnNextTurn;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnTurnEnd;
        GameplayEvents.GamePhaseChanged -= OnNextTurn;
    }
}