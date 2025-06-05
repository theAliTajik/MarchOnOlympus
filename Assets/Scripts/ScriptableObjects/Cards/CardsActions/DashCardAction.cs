using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashCardAction : BaseCardAction
{

    private int m_energyAmount = 0;
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        DashCard c = (DashCard)cardData;
        
        CombatManager.Instance.DrawCard(c.DrawAmount);
        CombatManager.Instance.ForceChangeStance(c.SwitchToStance);
        m_energyAmount = c.EnergyGain;
        GameplayEvents.GamePhaseChanged += OnNextTurnStart;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
                
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    private void OnNextTurnStart(EGamePhase phase)
    {
        if (phase == EGamePhase.PLAYER_TURN_START)
        {
            CombatManager.Instance.Energy.GainEnergy(m_energyAmount);
            GameplayEvents.GamePhaseChanged -= OnNextTurnStart;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnNextTurnStart;
    }

}