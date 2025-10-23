using System;
using Game.ModifiableParam;

public class ClampValueModifier<T> : IParamModifier<T> where T : IComparable<T>
{
    public ClampValueModifier(T min, T max)
    {
        m_bottomValue = min;
        m_topValue = max;
    }

    public ClampValueModifier(T min)
    {
        m_bottomValue = min;
        m_clampTop = false;
    }
    
    public int Priority { get; set; }
    private T m_bottomValue;
    private T m_topValue;

    private bool m_clampTop = true;
    
    public T Modify(T value)
    {
        if (value.CompareTo(m_bottomValue) < 0)
            return m_bottomValue;
        if (value.CompareTo(m_topValue) > 0 && m_clampTop)
            return m_topValue;
        
        return value;
    }
}
