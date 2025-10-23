

using UnityEngine.Serialization;

public class AllOutPerkData : BasePerkData
{
    [FormerlySerializedAs("actionType")] public CardActionType cardActionType;
    public int EnergyGain;
}
