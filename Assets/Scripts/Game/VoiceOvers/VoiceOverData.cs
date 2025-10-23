
using System;using UnityEngine;

public enum VoiceOverTriggerType
{
    OnPlayerDeath,
    OnCombatEntry,
    OnPlayer66Percent,
    OnPlayer33Percent,
}

[Serializable]
public class VoiceOverData : ScriptableObject
{
    public VoiceOverTriggerType triggerType;
    public SoundEffectSO soundEffect;
}
