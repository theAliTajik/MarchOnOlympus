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
    protected int m_stack;
    protected Fighter m_fighter;
    protected bool m_isPlayer;
    
    protected bool m_hasGuard;
    protected int m_guardMin;
    
    public int Stack
    {
        get => m_stack;
        //set => m_stack = value;
    }
    public Fighter Fighter => m_fighter;
    
    public bool HasGuard { get => m_hasGuard;}
    public int GuardMin { get => m_guardMin;}
    
    public abstract MechanicType GetMechanicType();

    public virtual Fighter GetFighter()
    {
        return m_fighter;
    }
    
    public void SetGuard(int minHP)
    {
        m_hasGuard = true;
        m_guardMin = minHP;
    }

    public void RemoveGuard()
    {
        m_hasGuard = false;
        m_guardMin = 0;
    }
    
    public abstract bool TryReduceStack(CombatPhase phase, bool isMyTurn);

    public virtual void ReduceStack(int amount)
    {
        if (m_hasGuard && m_stack - amount < m_guardMin)
        {
            amount = m_stack - m_guardMin;
        }

        if (amount < 0)
        {
            return;
        }
        m_stack -= amount;
        if (m_stack <= 0)
        {
            RaiseOnEnd();
            return;
        }
        RaiseOnChange();
    }

    public virtual void IncreaseStack(int amount)
    {
        if (amount < 0)
        {
            return;
        }
        m_stack += amount;
        RaiseOnChange();
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
