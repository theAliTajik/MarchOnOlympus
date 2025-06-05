
using System;
using System.Linq.Expressions;
using Game.ModifiableParam;
static class Operator<T>
{
    public static readonly Func<T, T, T> Add;

    static Operator()
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");
        var body = Expression.Add(paramA, paramB);
        Add = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
    }
}

public class AddValueModifier<T> : IParamModifier<T>
{
    public AddValueModifier(T firstValue)
    {
        m_firstValue = firstValue;
    }
    
    public int Priority { get; set; }

    private T m_firstValue;
    public T Modify(T value)
    {
        return Operator<T>.Add(m_firstValue, value);
    }
}
