using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{

    [SerializeField] private AudioMixer m_AudioMixer; 

    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";


    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PARAM, volume);
        // Convert volume (0-1) to decibels (-80 to 0)
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        m_AudioMixer.SetFloat(MUSIC_VOLUME_PARAM, dbVolume);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_PARAM, volume);
        // Convert volume (0-1) to decibels (-80 to 0)
        float dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20;
        m_AudioMixer.SetFloat(SFX_VOLUME_PARAM, dbVolume);
    }
    
    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, 1);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_PARAM, 1);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME_PARAM))
        {
            float Volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM);
            SetMusicVolume(Volume);
        }
        else
        {
            SetMusicVolume(0.5f);
        }


        if (PlayerPrefs.HasKey(SFX_VOLUME_PARAM))
        {
            float Volume = PlayerPrefs.GetFloat(SFX_VOLUME_PARAM);
            SetSFXVolume(Volume);
        }
        else
        {
            SetSFXVolume(0.5f);
        }
    }

    protected override void Init()
    {
        
    }
}
