
using Game.ModifiableParam;

public class MultiplyModifier : IParamModifier<int>
{
    public MultiplyModifier(int multplicant)
    {
        m_multplicant = multplicant;
    }

    public int Priority { get; set; } = 10;
    
    private int m_multplicant;
    
    public int Modify(int value)
    {
        return value * m_multplicant;
    }
}
