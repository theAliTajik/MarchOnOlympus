using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeThemBleedCardAction : BaseCardAction
{
    private MakeThemBleedCard m_card;
    private Fighter m_target;
    private bool m_firstTurn = true;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        m_card = (MakeThemBleedCard)cardData;
        m_target = target;
        
        
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {
        MechanicsManager.Instance.AddMechanic(new BleedMechanic(m_card.BleedAmount, m_target));

        CombatManager.Instance.OnCombatPhaseChanged += ApplyBleed;

        finishCallback?.Invoke();
        yield break;
    }

    private void ApplyBleed(CombatPhase phase)
    {
        //Debug.Log("apply bleed called");
        if (phase != CombatPhase.TURN_START || !CombatManager.Instance.IsPlayersTurn)
        {
            //Debug.Log("returned");
            return;
        }
        /*if (m_firstTurn)
        {
            Debug.Log("was first turn");
            m_firstTurn = false;
            return;
        }*/

        Debug.Log("applied 2 bleed");
        MechanicsManager.Instance.AddMechanic(new BleedMechanic(m_card.BleedAmount, m_target));
        
    }

}