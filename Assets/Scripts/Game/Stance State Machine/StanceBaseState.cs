using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class StanceBaseState
{
    protected StanceStateMachine m_context;
    protected int ReduceCdAmount = -1;
    
    public virtual void OnEnter()
    {
        m_context.SetCD(m_context.defaultStanceCD);
    }
    
    public virtual void OnExit() { }

    public virtual void OnTurnStart() 
    {
        if (!m_context.InfiniteCd)
        {
            m_context.SetCD(ReduceCdAmount, true);
            
            if (m_context.StanceCD <= 0)
            {
                m_context.ChangeState(Stance.NONE);
            }
        }
    }
    
    public virtual void OnMechanicAdded() { }

    public abstract Stance GetStance();

}
