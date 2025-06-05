using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Stance
{
    NONE,
    DEFENCIVE,
    BATTLE,
    BERSERKER
}

public class StanceStateMachine : MonoBehaviour
{
    private StanceBaseState m_currentState;
    private Stance m_currentStance;

    private StanceStateFactory m_stateFactory;
    
    public StanceBaseState CurrentState { get { return m_currentState; } }
    
    //data
    private int stanceCD;
    [SerializeField] private int DefaultStanceCD;
    private bool m_infiniteCd;
    private bool m_stanceChangeAllowedOverride;

    public Stance CurrentStance { get { return m_currentStance; } }
    public int StanceCD { get { return stanceCD; }}
    public int defaultStanceCD { get { return DefaultStanceCD; } }
    

    public bool InfiniteCd => m_infiniteCd;

    private void Awake()
    {
        m_stateFactory = new StanceStateFactory(this);
        m_currentState = m_stateFactory.GetState(Stance.NONE);
        m_currentState.OnEnter();
        
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }

    public void ChangeState(Stance nextState, bool OverrideCD = false)
    {
        if (nextState == m_currentStance)
        {
            return;
        }

        if (!OverrideCD && !IsStanceChangeAllowed()) 
        {
            return;
        }

        m_currentState.OnExit();
        
        m_currentState = m_stateFactory.GetState(nextState);
        m_currentStance = m_currentState.GetStance();
        
        m_currentState.OnEnter();
        
        GameplayEvents.SendStanceChanged(nextState);
    }

    public bool IsStanceChangeAllowed()
    {
        if (m_stanceChangeAllowedOverride)
        {
            return true;
        }
        
        if (StanceCD <= 0)
        {
            return true;
        }

        return false;
    }

    public void SetCD(int value, bool additive = false)
    {
        if (m_infiniteCd)
        {
            return;
        }
        
        if (additive)
        {
            stanceCD += value;
        }
        else
        {
            stanceCD = value;
        }

        if (stanceCD < 0)
        {
            stanceCD = 0;
        }
        
        GameplayEvents.SendOnStanceCdChanged(stanceCD);
    }

    public void ResetStanceCdToDefault()
    {
        SetCD(defaultStanceCD);
    }



    public void OnPhaseChange(EGamePhase phase)
    {
        switch (phase)
        {
            case EGamePhase.PLAYER_TURN_START:
                m_currentState.OnTurnStart();
                break;
            case EGamePhase.MECHANIC_ADDED:
                m_currentState.OnMechanicAdded();
                break;
        }
    }

    public void SetInfiniteCd(bool value)
    {
        m_infiniteCd = value;
        if (m_infiniteCd)
        {
            stanceCD = -1;
        }
        else
        {
            stanceCD = defaultStanceCD;
        }
        
        GameplayEvents.SendOnStanceCdChanged(stanceCD);

    }

    public void SetStanceChangeAllowed(bool value)
    {
        m_stanceChangeAllowedOverride = value;
    }
}
