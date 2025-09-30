using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioMixer; 
    // [SerializeField] private Transform m_soundPlayPosition;
    
    private void OnEnable()
    {
        SoundEffectsEventBus.OnSoundEffectPlay += PlaySound;
        SoundEffectsEventBus.OnSoundEffectPlayID += PlaySound;
    }

    private void OnDisable()
    {
        SoundEffectsEventBus.OnSoundEffectPlay -= PlaySound;
        SoundEffectsEventBus.OnSoundEffectPlayID -= PlaySound;
    }


    private void PlaySound(string ID)
    {
        var effect = SoundsDb.Instance?.Get(ID);
        if (effect != null)
        {
            PlaySound(effect);
        }
    }

    private void PlaySound(SoundEffectSO effect){
        if (effect == null)
        {
            CustomDebug.LogWarning("Sound effect was null", Categories.Sound.SFX);
            return;
        }

        var clip = effect.GetRandomClip();
        
        if (clip == null) {
            
            CustomDebug.LogWarning("Sound effect audio clip was null", Categories.Sound.SFX);
            return;
        }

        m_AudioMixer.PlayOneShot(clip);
    }
}
