
using Game.ModifiableParam;

public class SetValueModifier<T> : IParamModifier<T>
{
    public SetValueModifier(T value)
    {
        m_valueToSet = value;
    }
    public int Priority { get; set; }

    private T m_valueToSet;
    public T Modify(T value)
    {
        return m_valueToSet;
    }
}
