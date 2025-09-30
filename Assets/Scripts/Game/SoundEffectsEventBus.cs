using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffectsEventBus
{
    public static event System.Action<SoundEffectSO> OnSoundEffectPlay;
    public static event System.Action<string> OnSoundEffectPlayID;
    
    public static void SendPlay(SoundEffectSO soundEffectSO)
    {
        OnSoundEffectPlay?.Invoke(soundEffectSO);
    }
    
    public static void SendPlay(string SFID)
    {
        OnSoundEffectPlayID?.Invoke(SFID);
    }
}
