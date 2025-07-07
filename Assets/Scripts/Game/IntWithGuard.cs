
using System;

[Serializable]
public class IntWithGuard
{
    public IntWithGuard()
    {
        
    }

    public IntWithGuard(int value)
    {
        m_stack = value;
    }
    
    public event Action OnChange;
    public event Action OnZero;
    
    private int m_stack;
    
    private bool m_hasGuard;
    private int m_guardMin;

    public int Amount => m_stack;
    
    public bool HasGuard => m_hasGuard;
    public int GuardMin => m_guardMin;
    
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
            OnZero?.Invoke();
            return;
        }
        OnChange?.Invoke();
    }

    public virtual void IncreaseStack(int amount)
    {
        if (amount < 0)
        {
            return;
        }
        m_stack += amount;
        OnChange?.Invoke();
    }
    
    public static implicit operator int(IntWithGuard value)
    {
        return value.Amount;
    }
    
    public static implicit operator IntWithGuard(int value)
    {
        return new IntWithGuard(value);
    }
}
