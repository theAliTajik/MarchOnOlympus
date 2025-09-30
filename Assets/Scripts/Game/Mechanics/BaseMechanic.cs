using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Resources;
using Game;
using UnityEngine;

public abstract class BaseMechanic
{
    public event Action<MechanicType> OnEnd;
    public event Action<MechanicType> OnChange;
    protected IntWithGuard m_stack;
    protected IHaveMechanics m_mechanicOwner;
    
    
    public int Stack => m_stack;
    public IHaveMechanics MechanicOwner => m_mechanicOwner;
    
    public bool HasGuard => m_stack.HasGuard;
    public int GuardMin => m_stack.GuardMin;

    public BaseMechanic()
    {
        m_stack = new IntWithGuard();
        m_stack.OnChange += RaiseOnChange;
        m_stack.OnZero += RaiseOnEnd;
    }
    
    public abstract MechanicType GetMechanicType();

    public virtual IHaveMechanics GetMechanicOwner()
    {
        return m_mechanicOwner;
    }
    
    public void SetGuard(int minHP)
    {
        m_stack.SetGuard(minHP);
    }

    public void RemoveGuard()
    {
        m_stack.RemoveGuard();
    }
    
    public abstract bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false);

    public virtual void ReduceStack(int amount)
    {
        m_stack.ReduceStack(amount);
    }

    public virtual void IncreaseStack(int amount)
    {
        m_stack.IncreaseStack(amount);
    }

    public virtual void Apply(Fighter.DamageContext context)
    {
        //default implementation does nothing
    }

    protected void RaiseOnEnd()
    {
        OnEnd?.Invoke(GetMechanicType());
    }
    
    protected void RaiseOnChange()
    {
        OnChange?.Invoke(GetMechanicType());
    }

}
