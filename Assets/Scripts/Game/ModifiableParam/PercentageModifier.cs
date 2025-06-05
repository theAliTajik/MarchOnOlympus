
using Game.ModifiableParam;
using UnityEngine;

public class PercentageModifier : IParamModifier<int>
{
    public PercentageModifier()
    {
        
    }
    public PercentageModifier(int percentage)
    {
        m_precentage = percentage;
    }
    public int Priority { get; set; }

    public int m_precentage;
    public int Modify(int value)
    {
        return Mathf.RoundToInt(value * (m_precentage /100f));
    }
}
