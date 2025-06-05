

using UnityEngine.Serialization;

public class EmergencyPerkData : BasePerkData
{
    public int HPThresholdPercentage;
    public int Restore;

    [FormerlySerializedAs("ConditionHasNotBenMet")] public bool ConditionHasBenMet;
}
