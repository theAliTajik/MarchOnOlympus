using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { Additive, Multiplicative, Override }

public class ModifyStatCommand : ICommand
{
    private int m_value;
    private ModifierType m_type;
    private Stat m_stat;
    
    private bool m_isUndone = false;

    public ModifyStatCommand(int value, ModifierType type, Stat stat)
    {
        m_value = value;
        m_type = type;
        m_stat = stat;
    }
    
    private int m_valueBeforeModify;
    
    public void Execute()
    {
        switch (m_type)
        {
            case ModifierType.Additive:
                m_valueBeforeModify = m_stat.GetValue();
                m_stat.SetValueOverride(m_value, true);
                break;
        }
    }

    public void Undo()
    {
        switch (m_type)
        {
            case ModifierType.Additive:
                int statValue = m_stat.GetValue();
                if (statValue <= m_valueBeforeModify)
                {
                    return;
                }
                
                int valueDiff = Mathf.Min(m_value, statValue - m_valueBeforeModify);
                m_stat.SetValueOverride(-valueDiff, true);
                break;
        }
        m_isUndone = true;
    }

    public string GetDescription()
    {
        switch (m_type)
        {
            case ModifierType.Additive:
                return "Additive : " + m_value;
            break;
        }
        
        return "does not provide description";
    }
}
